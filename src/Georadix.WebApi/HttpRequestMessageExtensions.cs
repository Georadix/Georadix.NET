namespace System.Net.Http
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    /// Defines methods that extend the <see cref="HttpRequestMessage"/> class.
    /// </summary>
    public static class HttpRequestMessageExtensions
    {
        private const string ForwardedFor = "X-Forwarded-For";
        private const string HttpContext = "MS_HttpContext";
        private const string OwinContext = "MS_OwinContext";
        private const string RemoteEndpointMessage = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";

        /// <summary>
        /// Gets the IP host address of the remote client, taking the X-Forwarded-For HTTP header into consideration
        /// in load-balancing scenarios.
        /// </summary>
        /// <param name="request">The request message.</param>
        /// <returns>
        /// A <see cref="string"/> containing the client IP address if it can be derived; otherwise,
        /// <see langword="null"/>.
        /// </returns>
        public static string GetClientIpAddress(this HttpRequestMessage request)
        {
            var address = request.GetHostDependentClientIpAddress();

            var forwardedFor = request.Headers.Contains(ForwardedFor) ? request.Headers.GetValues(ForwardedFor) : null;

            // If the X-Forwarded-For HTTP header is not present, return the user host address.
            if ((forwardedFor == null) || !forwardedFor.Any())
            {
                return address;
            }

            // Get a list of public IP addresses in the X-Forwarded-For header.
            var publicForwardingIps = forwardedFor.Where(ip =>
                {
                    IPAddress parsedId;
                    return IPAddress.TryParse(ip, out parsedId) ? !parsedId.IsPrivate() : false;
                });

            // If we find any, return the first one. Otherwise, return the user host address.
            return publicForwardingIps.Any() ? publicForwardingIps.First() : address;
        }

        /// <summary>
        /// Gets the IP host address of the remote client, whether Web host, self host, or OWIN is used for the API.
        /// </summary>
        /// <param name="request">The request message.</param>
        /// <returns>
        /// A <see cref="string"/> containing the client IP address if it can be derived; otherwise,
        /// <see langword="null"/>.
        /// </returns>
        [ExcludeFromCodeCoverage]
        private static string GetHostDependentClientIpAddress(this HttpRequestMessage request)
        {
            // Web host.
            if (request.Properties.ContainsKey(HttpContext))
            {
                dynamic httpContext = request.Properties[HttpContext];
                return httpContext.Request.UserHostAddress;
            }

            // Self host.
            if (request.Properties.ContainsKey(RemoteEndpointMessage))
            {
                dynamic remoteEndpoint = request.Properties[RemoteEndpointMessage];
                return remoteEndpoint.Address;
            }

            // Self host using OWIN.
            if (request.Properties.ContainsKey(OwinContext))
            {
                dynamic owinContext = request.Properties[OwinContext];
                return owinContext.Request.RemoteIpAddress;
            }

            return null;
        }
    }
}