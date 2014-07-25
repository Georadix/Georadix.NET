namespace Georadix.Web
{
    using Georadix.Web.Resources;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IdentityModel.Tokens;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Web;

    /// <summary>
    /// Represents an <see cref="IHttpModule"/> that handles authorization based on JWT.
    /// </summary>
    public class JwtValidationModule : IHttpModule
    {
        private TokenValidationParameters tokenValidationParameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtValidationModule"/> class.
        /// </summary>
        public JwtValidationModule()
        {
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements
        /// <see cref="IHttpModule" />.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public virtual void Dispose()
        {
        }

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">
        /// An <see cref="HttpApplication" /> that provides access to the methods,
        /// properties, and events common to all application objects within an ASP.NET application
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="context"/> does not implement <see cref="IJwtValidationParametersProvider"/>.
        /// </exception>
        public void Init(HttpApplication context)
        {
            context.AssertNotNull("context");

            var app = context as IJwtValidationParametersProvider;

            if (app == null)
            {
                throw new InvalidOperationException(InvariantStrings.InvalidHttpApplication);
            }

            this.tokenValidationParameters = app.TokenValidationParameters;

            context.AuthenticateRequest += this.Application_AuthenticateRequest;
        }

        /// <summary>
        /// Gets the principal by examining the request for a JWT.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <remarks>
        /// Called when the <see cref="HttpApplication" /> fires the authenticate request event.
        /// </remarks>
        /// <returns>
        /// If an access token is present, returns a <see cref="ClaimsPrincipal"/> representing the user
        /// who made the request; otherwise, an anonymous <see cref="ClaimsPrincipal"/>.
        /// </returns>
        protected virtual IPrincipal GetPrincipalFromRequest(HttpRequest request)
        {
            var principal = new ClaimsPrincipal(new ClaimsIdentity());
            var token = this.GetTokenFromRequest(request);

            if (!string.IsNullOrWhiteSpace(token))
            {
                try
                {
                    principal = this.ValidateToken(token);
                }
                catch (Exception ex)
                {
                    this.OnValidateTokenException(request, ex);
                }
            }

            return principal;
        }

        /// <summary>
        /// Gets the token from the request by looking at the authorization header.
        /// </summary>
        /// <remarks>
        /// Override this method if your mechanism for sending access token differs, for example using cookies.
        /// </remarks>
        /// <param name="request">The request.</param>
        /// <returns>
        /// If the authorization header is present, a string containing the token; otherwise, <see langword="null"/>.
        /// </returns>
        protected virtual string GetTokenFromRequest(HttpRequest request)
        {
            string token = null;

            var authHeader = request.Headers["Authorization"];

            if (authHeader != null && !string.IsNullOrWhiteSpace(authHeader))
            {
                var parts = authHeader.Split(' ');
                if ((parts.Length == 2) && (parts[0] == "Bearer"))
                {
                    token = parts[1];
                }
            }

            return token;
        }

        /// <summary>
        /// Called when validating an access token fails.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="ex">The exception.</param>
        [ExcludeFromCodeCoverage]
        protected virtual void OnValidateTokenException(HttpRequest request, Exception ex)
        {
        }

        [ExcludeFromCodeCoverage]
        private void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            var application = (HttpApplication)sender;

            application.Context.User = this.GetPrincipalFromRequest(application.Context.Request);
        }

        private ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.ValidateToken(new JwtSecurityToken(token), this.tokenValidationParameters);
        }
    }
}