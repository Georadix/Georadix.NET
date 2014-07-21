namespace Georadix.Web.Tests
{
    using System.Web;
    using Xunit;

    public class HttpRequestExtensionsFixture
    {
        private const string ForwardedFor = "X-Forwarded-For";
        private const string GoogleIpAddress = "74.125.224.224";
        private const string MalformedIpAddress = "MALFORMED";
        private const string MicrosoftIpAddress = "65.55.58.201";
        private const string Private16Bit = "192.168.0.0";
        private const string Private20Bit = "172.16.0.0";
        private const string Private24Bit = "10.0.0.0";
        private const string PrivateLinkLocal = "169.254.0.0";

        [Fact]
        public void GetClientIpAddressForEmptyXForwardedForReturnsNull()
        {
            var sut = new HttpRequest(string.Empty, "http://www.example.com", string.Empty);

            sut.AddHeader(ForwardedFor, string.Empty);

            Assert.Equal(null, sut.GetClientIpAddress());
        }

        [Fact]
        public void GetClientIpAddressForMultiplePublicProxiesReturnsFirstPublicProxyIp()
        {
            var sut = new HttpRequest(string.Empty, "http://www.example.com", string.Empty);

            sut.AddHeader(
                ForwardedFor,
                Private16Bit,
                Private20Bit,
                Private24Bit,
                MicrosoftIpAddress,
                GoogleIpAddress,
                PrivateLinkLocal);

            Assert.Equal(MicrosoftIpAddress, sut.GetClientIpAddress());
        }

        [Fact]
        public void GetClientIpAddressForNullXForwardedForReturnsNull()
        {
            var sut = new HttpRequest(string.Empty, "http://www.example.com", string.Empty);

            Assert.Equal(null, sut.GetClientIpAddress());
        }

        [Fact]
        public void GetClientIpAddressForPartiallyMalformedXForwardedForReturnsProperlyFormedClientIp()
        {
            var sut = new HttpRequest(string.Empty, "http://www.example.com", string.Empty);

            sut.AddHeader(
                ForwardedFor,
                MalformedIpAddress,
                GoogleIpAddress,
                MalformedIpAddress);

            Assert.Equal(GoogleIpAddress, sut.GetClientIpAddress());
        }

        [Fact]
        public void GetClientIpAddressForPublicClientAndMultipleProxiesReturnsPublicClientIp()
        {
            var sut = new HttpRequest(string.Empty, "http://www.example.com", string.Empty);

            sut.AddHeader(
                ForwardedFor,
                MicrosoftIpAddress,
                Private16Bit,
                Private20Bit,
                Private24Bit,
                PrivateLinkLocal);

            Assert.Equal(MicrosoftIpAddress, sut.GetClientIpAddress());
        }
    }
}