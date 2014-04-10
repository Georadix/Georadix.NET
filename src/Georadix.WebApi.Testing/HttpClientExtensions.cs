namespace Georadix.WebApi.Testing
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Protocols.WSTrust;
    using System.IdentityModel.Tokens;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Claims;
    using System.Security.Cryptography.X509Certificates;

    /// <summary>
    /// Extensions on the <see cref="HttpClient"/> class to facilitate authenticating requests.
    /// </summary>
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Sets a JWT authorization header on the default request headers of an <see cref="HttpClient"/>.
        /// </summary>
        /// <param name="client">The client for which to set the authorization header.</param>
        /// <param name="signingCertificate">The signing certificate to sign the token.</param>
        /// <param name="appliesToAddress">The address for which the token is considered valid.</param>
        /// <param name="claims">The claims that define the user. Leave null for an anonymous user.</param>
        /// <param name="tokenIssuerName">Name of the token issuer. Defaults to "self".</param>
        /// <param name="tokenDuration">
        /// The token duration for which it's considered valid. Defaults to 2 hours.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="signingCertificate"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="appliesToAddress"/> is <see langword="null"/> or empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="tokenIssuerName"/> is <see langword="null"/> or empty.
        /// </exception>
        public static void SetJwtAuthorizationHeader(
            this HttpClient client,
            X509Certificate2 signingCertificate,
            string appliesToAddress,
            IEnumerable<Claim> claims = null,
            string tokenIssuerName = "self",
            TimeSpan? tokenDuration = null)
        {
            signingCertificate.AssertNotNull("signingCertificate");
            appliesToAddress.AssertNotNullOrWhitespace("appliesToAddress");
            tokenIssuerName.AssertNotNullOrWhitespace("tokenIssuerName");

            var now = DateTime.UtcNow;
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                TokenIssuerName = tokenIssuerName,
                AppliesToAddress = appliesToAddress,
                Lifetime = new Lifetime(now, now.Add(tokenDuration ?? TimeSpan.FromHours(2))),
                SigningCredentials = new X509SigningCredentials(signingCertificate)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
        }
    }
}