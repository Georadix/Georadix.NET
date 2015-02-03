namespace System.Net
{
    /// <summary>
    /// Defines methods and properties that extend System.Net types.
    /// </summary>
    public static class NetExtensions
    {
        /// <summary>
        /// Determines whether an IP address is in the private IP address space.
        /// </summary>
        /// <remarks>
        /// http://en.wikipedia.org/wiki/Private_network
        /// The private IP addresses are:
        ///  24-bit block: 10.0.0.0 through 10.255.255.255
        ///  20-bit block: 172.16.0.0 through 172.31.255.255
        ///  16-bit block: 192.168.0.0 through 192.168.255.255
        ///  Link-local block: 169.254.0.0 through 169.254.255.255
        /// </remarks>
        /// <param name="ip">The IP address.</param>
        /// <returns>
        /// <c>true</c> if the IP address is in the private IP address space; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPrivate(this IPAddress ip)
        {
            var octets = ip.GetAddressBytes();

            // 24-bit block.
            if (octets[0] == 10)
            {
                return true;
            }

            // 20-bit block.
            if ((octets[0] == 172) && (octets[1] >= 16) && (octets[1] <= 31))
            {
                return true;
            }

            // 16-bit block.
            if ((octets[0] == 192) && (octets[1] == 168))
            {
                return true;
            }

            // Link-local block.
            return (octets[0] == 169) && (octets[1] == 254);
        }
    }
}