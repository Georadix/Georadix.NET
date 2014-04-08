namespace Georadix.WebApi
{
    using System.Security.Cryptography.X509Certificates;

    /// <summary>
    /// A provider contract that gives access to the authentication certificate used to sign security tokens.
    /// </summary>
    public interface IAuthCertificateProvider
    {
        /// <summary>
        /// Gets the URL for which security tokens are considered valid.
        /// </summary>
        string AllowedAudience { get; }

        /// <summary>
        /// Gets the certificate.
        /// </summary>
        X509Certificate2 Certificate { get; }
    }
}