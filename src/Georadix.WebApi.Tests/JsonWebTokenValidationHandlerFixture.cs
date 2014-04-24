namespace Georadix.WebApi
{
    using Georadix.WebApi.Testing;
    using log4net;
    using Moq;
    using SimpleInjector;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.IdentityModel.Protocols.WSTrust;
    using System.IdentityModel.Tokens;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Claims;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Xunit;

    public class JsonWebTokenValidationHandlerFixture
    {
        private readonly List<string> allowedAudiences = new List<string>(new string[] { "http://www.example.com" });
        private readonly X509Certificate2 certificate;
        private readonly string rootRoute;

        public JsonWebTokenValidationHandlerFixture()
        {
            var store = new X509Store(StoreName.TrustedPeople);

            store.Open(OpenFlags.ReadOnly);

            var certName = ConfigurationManager.AppSettings["AuthCertName"];

            this.certificate = store.Certificates.Cast<X509Certificate2>()
                .Single(c => c.Subject.Equals(certName));

            this.rootRoute = string.Format(
                "{0}/entities", typeof(JsonWebTokenValidationHandlerFixtureController).Name.ToLowerInvariant());
        }

        [Fact]
        public async Task AccessingAnonymousResourceWithNoTokenReturnsOK()
        {
            var loggerMock = new Mock<ILog>(MockBehavior.Strict);

            using (var server = this.CreateServer(loggerMock.Object))
            {
                var response = await server.Client.GetAsync(this.rootRoute);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task AccessingProtectedResourceWithNoTokenReturnsForbidden()
        {
            var loggerMock = new Mock<ILog>(MockBehavior.Strict);

            using (var server = this.CreateServer(loggerMock.Object))
            {
                var response = await server.Client.PostAsJsonAsync<string>(this.rootRoute, "test");

                Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            }
        }

        [Fact]
        public async Task AccessingProtectedResourceWithValidTokenReturnsIt()
        {
            var loggerMock = new Mock<ILog>(MockBehavior.Strict);

            using (var server = this.CreateServer(loggerMock.Object))
            {
                server.Client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", this.GenerateAuthToken(this.allowedAudiences.First()));

                var response = await server.Client.PostAsJsonAsync<string>(this.rootRoute, "test");

                var content = await response.Content.ReadAsAsync<string>();

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal("test", content);
            }
        }

        [Fact]
        public void ConstructorWithNullCertificateThrowsArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new JsonWebTokenValidationHandler(Mock.Of<ILog>(), this.allowedAudiences, null));

            Assert.Equal("certificate", ex.ParamName);
        }

        [Fact]
        public void ConstructorWithNullLoggerThrowsArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new JsonWebTokenValidationHandler(null, this.allowedAudiences, this.certificate));

            Assert.Equal("logger", ex.ParamName);
        }

        [Fact]
        public void ConstrutorWithEmptyAllowedAudiencesThrowsArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(
                () => new JsonWebTokenValidationHandler(Mock.Of<ILog>(), new string[] { }, this.certificate));

            Assert.Equal("allowedAudiences", ex.ParamName);
        }

        [Fact]
        public void ConstrutorWithNullAllowedAudiencesThrowsArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new JsonWebTokenValidationHandler(Mock.Of<ILog>(), null, this.certificate));

            Assert.Equal("allowedAudiences", ex.ParamName);
        }

        [Fact]
        public async Task RequestWithMalformedTokenIsLogged()
        {
            var loggerMock = new Mock<ILog>(MockBehavior.Strict);

            loggerMock.Setup(l => l.Info(
                It.Is<string>(s => s.Equals("The auth token from client IP  is invalid.")),
                It.IsAny<Exception>()));

            using (var server = this.CreateServer(loggerMock.Object))
            {
                server.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "dfggd");

                var response = await server.Client.PostAsJsonAsync<string>(this.rootRoute, "test");

                Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

                loggerMock.VerifyAll();
            }
        }

        private InMemoryServer CreateServer(ILog logger)
        {
            var server = new InMemoryServer();

            server.Configuration.MessageHandlers.Add(
                new JsonWebTokenValidationHandler(logger, this.allowedAudiences, this.certificate));

            server.Configuration.MapHttpAttributeRoutes();

            return server;
        }

        private string GenerateAuthToken(string audience)
        {
            var now = DateTime.UtcNow;
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "Username"),
                        new Claim(ClaimTypes.Role, "User"),
                    }),
                TokenIssuerName = "self",
                AppliesToAddress = audience,
                Lifetime = new Lifetime(now, now.AddMinutes(2)),
                SigningCredentials = new X509SigningCredentials(this.certificate)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }

    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Embedding this controller class inside the fixture causes routing to its methods to fail.")]
    [RoutePrefix("jsonwebtokenvalidationhandlerfixturecontroller/entities")]
    public class JsonWebTokenValidationHandlerFixtureController : ApiController
    {
        [AllowAnonymous]
        [Route("")]
        public string Get()
        {
            return "test";
        }

        [Authorize]
        [Route("")]
        public string Post([FromBody] string value)
        {
            return value;
        }
    }
}