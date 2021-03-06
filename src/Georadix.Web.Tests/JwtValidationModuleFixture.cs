﻿namespace Georadix.Web.Tests
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
    using Xunit.Extensions;

    public class JwtValidationModuleFixture
    {
        private readonly List<string> allowedAudiences = new List<string>(new string[] { "http://www.example.com" });
        private readonly X509Certificate2 certificate;

        public JwtValidationModuleFixture()
        {
            var store = new X509Store(StoreName.TrustedPeople);

            store.Open(OpenFlags.ReadOnly);

            var certName = ConfigurationManager.AppSettings["AuthCertName"];

            this.certificate = store.Certificates.Cast<X509Certificate2>()
                .Single(c => c.Subject.Equals(certName));
        }

        [Fact]
        public void GetTokenFromRequestWithAuthorizationHeaderReturnsToken()
        {
            var sut = new TestModule();
            var request = new HttpRequest(string.Empty, "http://www.example.com", string.Empty);
            var expectedToken = "access-token";

            request.AddHeader("Authorization", "Bearer " + expectedToken);

            var extractedToken = sut.GetTokenFromRequestTest(request);

            Assert.Equal(expectedToken, extractedToken);
        }

        [Theory]
        [InlineData("Bearing access-token")]
        [InlineData("Bearer access token")]
        [InlineData("access-token")]
        public void GetTokenFromRequestWithInvalidAuthorizationHeaderReturnsNull(string header)
        {
            var sut = new TestModule();
            var request = new HttpRequest(string.Empty, "http://www.example.com", string.Empty);

            request.AddHeader("Authorization", header);

            var extractedToken = sut.GetTokenFromRequestTest(request);

            Assert.Null(extractedToken);
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
        public void InitWithInvalidContextThrowsInvalidOperationException()
        {
            var sut = new JwtValidationModule();

            Assert.Throws<InvalidOperationException>(() => sut.Init(new HttpApplication()));
        }

        [Fact]
        public void InitWithNullContextThrowsArgumentNullException()
        {
            var sut = new JwtValidationModule();

            var ex = Assert.Throws<ArgumentNullException>(() => sut.Init(null));

            Assert.Equal("context", ex.ParamName);
        }

        [Fact]
        public void InitWithValidContextSucceeds()
        {
            var sut = new JwtValidationModule();
            var testApp = new TestApplication(new TokenValidationParameters()
                {
                    AllowedAudiences = this.allowedAudiences,
                    SigningToken = new X509SecurityToken(this.certificate),
                    ValidIssuer = "self"
                });

            sut.Init(testApp);

            // TODO: Find a way to verify we are properly registering to AuthenticateRequest.
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

            request.AddHeader("Authorization", "Bearer invalid-token");

            var sut = new TestModule();

            sut.Init(application);

            var principal = (ClaimsPrincipal)sut.GetPrincipalFromRequestTest(request);

            Assert.NotNull(sut.ValidationTokenException);
            Assert.False(principal.Identity.IsAuthenticated);
            Assert.Empty(principal.Claims);
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

            request.AddHeader("Authorization", "Bearer " + this.GenerateAuthToken("http://www.example.com"));

            var sut = new TestModule();

            sut.Init(application);

            var principal = (ClaimsPrincipal)sut.GetPrincipalFromRequestTest(request);

            Assert.True(principal.Identity.IsAuthenticated);
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

        private class TestApplication : HttpApplication, IJwtValidationParametersProvider
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

        private class TestModule : JwtValidationModule
        {
            public Exception ValidationTokenException { get; private set; }

            public IPrincipal GetPrincipalFromRequestTest(HttpRequest request)
            {
                return this.GetPrincipalFromRequest(request);
            }

            public string GetTokenFromRequestTest(HttpRequest request)
            {
                return this.GetTokenFromRequest(request);
            }

            protected override void OnValidateTokenException(HttpRequest request, Exception ex)
            {
                this.ValidationTokenException = ex;
            }
        }
    }
}