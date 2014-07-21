namespace Georadix.WebApi
{
    using System;
    using System.IdentityModel.Tokens;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A handler that validates a Web Token is present and valid for all requests.
    /// </summary>
    public class JsonWebTokenValidationHandler : DelegatingHandler
    {
        private readonly TokenValidationParameters tokenValidationParameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonWebTokenValidationHandler" /> class.
        /// </summary>
        /// <param name="tokenValidationParameters">The token validation parameters.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="tokenValidationParameters" /> is <see langword="null" />.
        /// </exception>
        public JsonWebTokenValidationHandler(TokenValidationParameters tokenValidationParameters)
        {
            tokenValidationParameters.AssertNotNull("tokenValidationParameters");

            this.tokenValidationParameters = tokenValidationParameters;
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

            var token = this.GetTokenFromRequest(request);

            if (!string.IsNullOrWhiteSpace(token))
            {
                try
                {
                    principal = this.ValidateToken(token);
                }
                catch (Exception)
                {
                }
            }

            request.GetRequestContext().Principal = principal;

            return base.SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// Gets the token from the request by looking at the authorization header.
        /// </summary>
        /// <remarks>
        /// Override this method if your mechanism for sending access token differs, for example using cookies.
        /// </remarks>
        /// <param name="request">The request.</param>
        /// <returns>
        /// If the authorization header is present, a string containing the token, otherwise <see langword="null"/>.
        /// </returns>
        protected virtual string GetTokenFromRequest(HttpRequestMessage request)
        {
            string token = null;
            var authorizationHeader = request.Headers.Authorization;

            if (authorizationHeader != null && !string.IsNullOrWhiteSpace(authorizationHeader.Parameter))
            {
                token = authorizationHeader.Parameter.Replace("Bearer ", string.Empty);
            }

            return token;
        }

        private ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.ValidateToken(new JwtSecurityToken(token), this.tokenValidationParameters);
        }
    }
}