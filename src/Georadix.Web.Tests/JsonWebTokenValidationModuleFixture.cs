namespace Georadix.Web.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IdentityModel.Protocols.WSTrust;
    using System.IdentityModel.Tokens;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Cryptography.X509Certificates;
    using System.Security.Principal;
    using System.Web;
    using Xunit;

    public class JsonWebTokenValidationModuleFixture
    {
        private readonly List<string> allowedAudiences = new List<string>(new string[] { "http://www.example.com" });
        private readonly X509Certificate2 certificate;

        public JsonWebTokenValidationModuleFixture()
        {
            var store = new X509Store(StoreName.TrustedPeople);

            store.Open(OpenFlags.ReadOnly);

            var certName = ConfigurationManager.AppSettings["AuthCertName"];

            this.certificate = store.Certificates.Cast<X509Certificate2>()
                .Single(c => c.Subject.Equals(certName));
        }

        [Fact]
        public void InitWithNullContextThrowsArgumentNullException()
        {
            var sut = new JsonWebTokenValidationModule();

            var ex = Assert.Throws<ArgumentNullException>(() => sut.Init(null));

            Assert.Equal("context", ex.ParamName);
        }

        [Fact]
        public void InitWithInvalidContextThrowsInvalidOperationException()
        {
            var sut = new JsonWebTokenValidationModule();

            Assert.Throws<InvalidOperationException>(() => sut.Init(new HttpApplication()));
        }

        [Fact]
        public void InitWithValidContextSucceeds()
        {
            var sut = new JsonWebTokenValidationModule();
            var testApp = new TestApplication(new TokenValidationParameters()
                {
                    AllowedAudiences = this.allowedAudiences,
                    SigningToken = new X509SecurityToken(this.certificate),
                    ValidIssuer = "self"
                });

            sut.Init(testApp);

            // TODO: Look at a way to mock an HttpApplication.
        }

        [Fact]
        public void GetTokenFromRequestWithoutAuthorizationHeaderReturnsNull()
        {
            var sut = new TestModule();
            var request = new HttpRequest(string.Empty, "http://www.example.com", string.Empty);

            var extractedToken = sut.GetTokenFromRequestTest(request);

            Assert.Null(extractedToken);
        }

        [Fact]
        public void GetTokenFromRequestWithAuthorizationHeaderReturnsToken()
        {
            var sut = new TestModule();
            var request = new HttpRequest(string.Empty, "http://www.example.com", string.Empty);
            var expectedToken = "access-token";

            request.AddHeader("Authorization", expectedToken);

            var extractedToken = sut.GetTokenFromRequestTest(request);

            Assert.Equal(expectedToken, extractedToken);
        }

        [Fact]
        public void OnAuthenticateRequestWithInvalidTokenCallsOnValidateTokenException()
        {
            var application = new TestApplication(new TokenValidationParameters()
            {
                AllowedAudiences = this.allowedAudiences,
                SigningToken = new X509SecurityToken(this.certificate),
                ValidIssuer = "self"
            });
            var request = new HttpRequest(string.Empty, "http://www.example.com", string.Empty);

            request.AddHeader("Authorization", "invalid token");

            var sut = new TestModule();

            sut.Init(application);

            var principal = (ClaimsPrincipal)sut.OnAuthenticateRequestTest(application, request);

            Assert.NotNull(sut.ValidationTokenException);
            Assert.False(principal.HasClaim(ClaimTypes.Name, "Username"));
            Assert.False(principal.HasClaim(ClaimTypes.Role, "User"));
        }

        [Fact]
        public void OnAuthenticateRequestWithTokenSetsApplicationContextUser()
        {
            var application = new TestApplication(new TokenValidationParameters()
            {
                AllowedAudiences = this.allowedAudiences,
                SigningToken = new X509SecurityToken(this.certificate),
                ValidIssuer = "self"
            });
            var request = new HttpRequest(string.Empty, "http://www.example.com", string.Empty);

            request.AddHeader("Authorization", this.GenerateAuthToken("http://www.example.com"));

            var sut = new TestModule();

            sut.Init(application);

            var principal = (ClaimsPrincipal)sut.OnAuthenticateRequestTest(application, request);

            Assert.True(principal.HasClaim(ClaimTypes.Name, "Username"));
            Assert.True(principal.HasClaim(ClaimTypes.Role, "User"));
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

        private class TestApplication : HttpApplication, IJsonWebTokenValidationParametersProvider
        {
            private readonly TokenValidationParameters tokenValidationParameters;

            public TestApplication(TokenValidationParameters tokenValidationParameters)
            {
                this.tokenValidationParameters = tokenValidationParameters;
            }

            public TokenValidationParameters TokenValidationParameters
            {
                get { return this.tokenValidationParameters; }
            }
        }

        private class TestModule : JsonWebTokenValidationModule
        {
            public Exception ValidationTokenException { get; private set; }

            public string GetTokenFromRequestTest(HttpRequest request)
            {
                return this.GetTokenFromRequest(request);
            }

            public IPrincipal OnAuthenticateRequestTest(HttpApplication application, HttpRequest request)
            {
                return this.OnAuthenticateRequest(application, request);
            }

            protected override void OnValidateTokenException(HttpRequest request, Exception ex)
            {
                this.ValidationTokenException = ex;
            }
        }
    }
}