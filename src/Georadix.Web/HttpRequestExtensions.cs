namespace System.Web
{
    using System.Linq;
    using System.Net;

    /// <summary>
    /// Defines methods that extend the <see cref="HttpRequest"/> class.
    /// </summary>
    public static class HttpRequestExtensions
    {
        private const string ForwardedFor = "X-Forwarded-For";

        /// <summary>
        /// Gets the IP host address of the remote client, taking the X-Forwarded-For HTTP header into consideration
        /// in load-balancing scenarios.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <returns>
        /// A <see cref="string"/> containing the client IP address if it can be derived; otherwise,
        /// <see langword="null"/>.
        /// </returns>
        public static string GetClientIpAddress(this HttpRequest request)
        {
            var address = request.UserHostAddress;

            var forwardedFor = request.Headers.GetValues(ForwardedFor);

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
    }
}