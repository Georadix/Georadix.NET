namespace Georadix.WebApi.Filters
{
    using Georadix.WebApi.Resources;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Controllers;

    /// <summary>
    /// Specifies a custom authorization filter that verifies the request's principal.
    /// </summary>
    /// <remarks>
    /// Only differs with the default <see cref="AuthorizeAttribute"/> by returning forbidden in cases where the client
    /// is authenticated but lacks the right to access a resource.
    /// </remarks>
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Processes requests that fail authorization.
        /// </summary>
        /// <param name="actionContext">The context.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="actionContext"/> is <see langword="null"/>.
        /// </exception>
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.AssertNotNull("actionContext");

            var principal = actionContext.RequestContext.Principal;

            if (!principal.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(actionContext);
            }
            else
            {
                actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(
                    HttpStatusCode.Forbidden, InvariantStrings.RequestNotAllowed);
            }
        }
    }
}