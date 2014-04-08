namespace Georadix.WebApi
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using Xunit;
    using Xunit.Extensions;

    public class AuthCertificateProviderFixture
    {
        private readonly X509Certificate2 certificate;

        public AuthCertificateProviderFixture()
        {
            var store = new X509Store(StoreName.TrustedPeople);

            store.Open(OpenFlags.ReadWrite);

            this.certificate = store.Certificates.Cast<X509Certificate2>()
                .Single(c => c.Subject.Equals(ConfigurationManager.AppSettings["AuthCertName"]));
        }

        [Fact]
        public void ConstructorReturnsInitializedInstance()
        {
            var sut = new AuthCertificateProvider(this.certificate.Subject, "audience");

            Assert.Equal(this.certificate.RawData, sut.Certificate.RawData);
            Assert.Equal("audience", sut.AllowedAudience);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void ConstructorWithInvalidAudienceThrowsArgumentException(string allowedAudience)
        {
            var ex = Assert.Throws<ArgumentException>(() => new AuthCertificateProvider("CN=Name", allowedAudience));

            Assert.Equal("allowedAudience", ex.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void ConstructorWithInvalidCertNameThrowsArgumentException(string certName)
        {
            var ex = Assert.Throws<ArgumentException>(() => new AuthCertificateProvider(certName, "audience"));

            Assert.Equal("certName", ex.ParamName);
        }

        [Fact]
        public void ConstructorWithMissingCertificateThrowsArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() => new AuthCertificateProvider("CN=Name", "audience"));

            Assert.Equal("certName", ex.ParamName);
            Assert.Contains("The certificate CN=Name is not in the TrustedPeople store.", ex.Message);
        }
    }
}