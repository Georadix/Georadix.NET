namespace Georadix.WebApi.Filters
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    /// <summary>
    /// Represents a filter attribute used to require requests to be sent using HTTPS.
    /// </summary>
    /// <remarks>
    /// If the request is not sent via HTTPS, a response is sent back with a body explaining where to find the
    /// requested resource. The caller is also automatically redirected, if possible.
    /// </remarks>
    public class RequireHttpsAttribute : AuthorizationFilterAttribute
    {
        private readonly int httpsPort;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequireHttpsAttribute"/> class with a specified HTTPS port.
        /// </summary>
        /// <param name="httpsPort">The HTTPS port.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="httpsPort"/> is less than 0.</exception>
        public RequireHttpsAttribute(int httpsPort = 443)
        {
            httpsPort.AssertGreaterThanOrEqualTo(0, "httpsPort");

            this.httpsPort = httpsPort;
        }

        /// <summary>
        /// Calls when a process requests authorization.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var request = actionContext.Request;

            if (request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                var uri = new UriBuilder(request.RequestUri);
                uri.Scheme = Uri.UriSchemeHttps;
                uri.Port = this.httpsPort;

                HttpResponseMessage response;
                string body = string.Format(
                    "<p>The resource can be found at <a href=\"{0}\">{0}</a>.</p>",
                    uri.Uri.AbsoluteUri);

                if (request.Method.Equals(HttpMethod.Get) || request.Method.Equals(HttpMethod.Head))
                {
                    response = request.CreateResponse(HttpStatusCode.Found);
                    response.Headers.Location = uri.Uri;

                    if (request.Method.Equals(HttpMethod.Get))
                    {
                        response.Content = new StringContent(body, Encoding.UTF8, "text/html");
                    }
                }
                else
                {
                    response = request.CreateResponse(HttpStatusCode.NotFound);
                    response.Content = new StringContent(body, Encoding.UTF8, "text/html");
                }

                actionContext.Response = response;
            }
        }
    }
}