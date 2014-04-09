﻿namespace Georadix.WebApi.Testing
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IdentityModel.Tokens;
    using System.Linq;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Security.Cryptography.X509Certificates;
    using Xunit;
    using Xunit.Extensions;

    public class HttpClientExtensionsFixture
    {
        private readonly X509Certificate2 certificate;

        public HttpClientExtensionsFixture()
        {
            var store = new X509Store(StoreName.TrustedPeople);

            store.Open(OpenFlags.ReadOnly);

            var certName = ConfigurationManager.AppSettings["AuthCertName"];

            this.certificate = store.Certificates.Cast<X509Certificate2>()
                .Single(c => c.Subject.Equals(certName));
        }

        [Fact]
        public void SetJwtAuthorizationHeaderCorrectly()
        {
            var sut = new HttpClient();

            var claim = new Claim(ClaimTypes.Name, "User1");

            sut.SetJwtAuthorizationHeader(this.certificate, "http://www.example.com", new Claim[] { claim });

            var principal = this.ValidateTokenWithX509SecurityToken(
                sut.DefaultRequestHeaders.Authorization.Parameter, new string[] { "http://www.example.com" });

            Assert.Equal("User1", principal.Identity.Name);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void SetJwtAuthorizationHeaderWithInvalidAddressThrowsArgumentNullException(string address)
        {
            var sut = new HttpClient();

            var ex = Assert.Throws<ArgumentException>(() => sut.SetJwtAuthorizationHeader(this.certificate, address));

            Assert.Equal("appliesToAddress", ex.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void SetJwtAuthorizationHeaderWithInvalidIssuerThrowsArgumentNullException(string issuer)
        {
            var sut = new HttpClient();

            var ex = Assert.Throws<ArgumentException>(
                () => sut.SetJwtAuthorizationHeader(this.certificate, "http://www.example.com", null, issuer));

            Assert.Equal("tokenIssuerName", ex.ParamName);
        }

        [Fact]
        public void SetJwtAuthorizationHeaderWithNullCertifiacteThrowsArgumentNullException()
        {
            var sut = new HttpClient();

            var ex = Assert.Throws<ArgumentNullException>(
                () => sut.SetJwtAuthorizationHeader(null, "http://www.example.com"));

            Assert.Equal("signingCertificate", ex.ParamName);
        }

        private ClaimsPrincipal ValidateTokenWithX509SecurityToken(string token, IEnumerable<string> allowedAudiences)
        {
            var x509DataClause = new X509RawDataKeyIdentifierClause(this.certificate.RawData);
            var tokenHandler = new JwtSecurityTokenHandler();
            var x509SecurityToken = new X509SecurityToken(new X509Certificate2(x509DataClause.GetX509RawData()));

            var validationParameters = new TokenValidationParameters()
            {
                AllowedAudiences = allowedAudiences,
                SigningToken = x509SecurityToken,
                ValidIssuer = "self"
            };

            return tokenHandler.ValidateToken(new JwtSecurityToken(token), validationParameters);
        }
    }
}