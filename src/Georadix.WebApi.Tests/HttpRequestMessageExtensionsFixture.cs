namespace Georadix.WebApi
{
    using System.Net.Http;
    using Xunit;

    public class HttpRequestMessageExtensionsFixture
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
            var sut = new HttpRequestMessage();
            sut.Headers.Add(ForwardedFor, string.Empty);

            Assert.Equal(null, sut.GetClientIpAddress());
        }

        [Fact]
        public void GetClientIpAddressForMultiplePublicProxiesReturnsFirstPublicProxyIp()
        {
            var sut = new HttpRequestMessage();
            sut.Headers.Add(ForwardedFor, Private16Bit);
            sut.Headers.Add(ForwardedFor, Private20Bit);
            sut.Headers.Add(ForwardedFor, Private24Bit);
            sut.Headers.Add(ForwardedFor, MicrosoftIpAddress);
            sut.Headers.Add(ForwardedFor, GoogleIpAddress);
            sut.Headers.Add(ForwardedFor, PrivateLinkLocal);

            Assert.Equal(MicrosoftIpAddress, sut.GetClientIpAddress());
        }

        [Fact]
        public void GetClientIpAddressForNullXForwardedForReturnsNull()
        {
            var sut = new HttpRequestMessage();

            Assert.Equal(null, sut.GetClientIpAddress());
        }

        [Fact]
        public void GetClientIpAddressForPartiallyMalformedXForwardedForReturnsProperlyFormedClientIp()
        {
            var sut = new HttpRequestMessage();
            sut.Headers.Add(ForwardedFor, MalformedIpAddress);
            sut.Headers.Add(ForwardedFor, GoogleIpAddress);
            sut.Headers.Add(ForwardedFor, MalformedIpAddress);

            Assert.Equal(GoogleIpAddress, sut.GetClientIpAddress());
        }

        [Fact]
        public void GetClientIpAddressForPublicClientAndMultipleProxiesReturnsPublicClientIp()
        {
            var sut = new HttpRequestMessage();
            sut.Headers.Add(ForwardedFor, MicrosoftIpAddress);
            sut.Headers.Add(ForwardedFor, Private16Bit);
            sut.Headers.Add(ForwardedFor, Private20Bit);
            sut.Headers.Add(ForwardedFor, Private24Bit);
            sut.Headers.Add(ForwardedFor, PrivateLinkLocal);

            Assert.Equal(MicrosoftIpAddress, sut.GetClientIpAddress());
        }
    }
}