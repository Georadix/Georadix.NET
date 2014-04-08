namespace Georadix.WebApi
{
    using Georadix.WebApi.Resources;
    using System;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;

    /// <summary>
    /// A provider that gives access to the authentication certificate used to sign security tokens.
    /// </summary>
    public class AuthCertificateProvider : IAuthCertificateProvider
    {
        private static X509Certificate2 certificate;
        private readonly string allowedAudience;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthCertificateProvider"/> class.
        /// </summary>
        /// <param name="certName">Name of the cert (CN=CertName).</param>
        /// <param name="allowedAudience">The allowed audience (URL format).</param>
        /// <exception cref="ArgumentException"><paramref name="certName"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">The certificate was not found in the trustedpeople store.</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="allowedAudience"/> is <see langword="null"/>.
        /// </exception>
        public AuthCertificateProvider(string certName, string allowedAudience)
        {
            certName.AssertNotNullOrWhitespace("certName");
            allowedAudience.AssertNotNullOrWhitespace("allowedAudience");

            this.allowedAudience = allowedAudience;

            var certStore = new X509Store(StoreName.TrustedPeople);

            certStore.Open(OpenFlags.ReadOnly);

            certificate = certStore.Certificates.Cast<X509Certificate2>()
                .SingleOrDefault(c => c.Subject.Equals(certName));

            if (certificate == null)
            {
                throw new ArgumentException(
                    string.Format(InvariantStrings.Error_CertificateNotFound, certName, StoreName.TrustedPeople),
                    "certName");
            }
        }

        /// <summary>
        /// Gets the URL for which security tokens are considered valid.
        /// </summary>
        public string AllowedAudience
        {
            get { return this.allowedAudience; }
        }

        /// <summary>
        /// Gets the certificate used to sign and validate authentication tokens.
        /// </summary>
        public X509Certificate2 Certificate
        {
            get { return certificate; }
        }
    }
}