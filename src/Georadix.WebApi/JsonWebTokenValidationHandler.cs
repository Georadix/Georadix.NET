namespace Georadix.WebApi
{
    using log4net;
    using System;
    using System.IdentityModel.Tokens;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Security.Cryptography.X509Certificates;
    using System.Security.Principal;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A handler that validates a Web Token is present and valid for all requests.
    /// </summary>
    public class JsonWebTokenValidationHandler : DelegatingHandler
    {
        private readonly IAuthCertificateProvider certProvider;
        private readonly ILog logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonWebTokenValidationHandler" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="certProvider">The certificate provider.</param>
        /// <exception cref="ArgumentNullException"><paramref name="logger"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="certProvider"/> is <see langword="null"/>.
        /// </exception>
        public JsonWebTokenValidationHandler(ILog logger, IAuthCertificateProvider certProvider)
        {
            logger.AssertNotNull("logger");
            certProvider.AssertNotNull("certProvider");

            this.logger = logger;
            this.certProvider = certProvider;
        }

        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        /// <returns>
        /// Returns <see cref="Task{HttpResponseMessage}" />. The task object representing the asynchronous operation.
        /// </returns>
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            IPrincipal principal = new ClaimsPrincipal();

            var authorizationHeader = request.Headers.Authorization;

            if (authorizationHeader != null && !string.IsNullOrWhiteSpace(authorizationHeader.Parameter))
            {
                var token = authorizationHeader.Parameter.Replace("Bearer ", string.Empty);

                if (!string.IsNullOrWhiteSpace(token))
                {
                    try
                    {
                        principal = this.ValidateTokenWithX509SecurityToken(
                            new X509RawDataKeyIdentifierClause(this.certProvider.Certificate.RawData), token);
                    }
                    catch (Exception e)
                    {
                        this.logger.Info("Invalid authentication token.", e);
                    }
                }
            }

            request.GetRequestContext().Principal = principal;

            return base.SendAsync(request, cancellationToken);
        }

        private ClaimsPrincipal ValidateTokenWithX509SecurityToken(
            X509RawDataKeyIdentifierClause x509DataClause, string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var x509SecurityToken = new X509SecurityToken(new X509Certificate2(x509DataClause.GetX509RawData()));

            var validationParameters = new TokenValidationParameters()
            {
                AllowedAudience = this.certProvider.AllowedAudience,
                SigningToken = x509SecurityToken,
                ValidIssuer = "self"
            };

            return tokenHandler.ValidateToken(new JwtSecurityToken(token), validationParameters);
        }
    }
}