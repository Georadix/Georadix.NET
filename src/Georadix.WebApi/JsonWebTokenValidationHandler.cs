namespace Georadix.WebApi
{
    using Georadix.WebApi.Resources;
    using log4net;
    using System;
    using System.Collections.Generic;
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
        private readonly List<string> allowedAudiences = new List<string>();
        private readonly X509Certificate2 certificate;
        private readonly ILog logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonWebTokenValidationHandler" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="allowedAudiences">A list of allowed audiences (Typically a list of domain names).</param>
        /// <param name="certificate">The certificate used to generate the signing token.</param>
        /// <exception cref="ArgumentNullException"><paramref name="logger"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="allowedAudiences"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="allowedAudiences"/> is empty.</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="certificate"/> is <see langword="null"/>.
        /// </exception>
        public JsonWebTokenValidationHandler(
            ILog logger, IEnumerable<string> allowedAudiences, X509Certificate2 certificate)
        {
            logger.AssertNotNull("logger");
            allowedAudiences.AssertNotNullOrEmpty(true, "allowedAudiences");
            certificate.AssertNotNull("certificate");

            this.logger = logger;
            this.allowedAudiences.AddRange(allowedAudiences);
            this.certificate = certificate;
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
                        principal = this.ValidateToken(token);
                    }
                    catch (Exception e)
                    {
                        this.logger.Info(
                            string.Format(InvariantStrings.LogInvalidAuthToken, request.GetClientIpAddress()), e);
                    }
                }
            }

            request.GetRequestContext().Principal = principal;

            return base.SendAsync(request, cancellationToken);
        }

        private ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters()
            {
                AllowedAudiences = this.allowedAudiences,
                SigningToken = new X509SecurityToken(this.certificate),
                ValidIssuer = "self"
            };

            return tokenHandler.ValidateToken(new JwtSecurityToken(token), validationParameters);
        }
    }
}