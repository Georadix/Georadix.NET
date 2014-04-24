namespace Georadix.Core
{
    using System.Net;
    using Xunit;
    using Xunit.Extensions;

    public class NetExtensionsFixture
    {
        [Theory]
        [InlineData("0.0.0.0", false)]
        [InlineData("12.66.131.122", false)]
        [InlineData("127.0.0.1", false)]
        [InlineData("155.12.87.143", false)]
        [InlineData("170.68.0.49", false)]
        [InlineData("188.99.66.33", false)]
        [InlineData("201.147.92.234", false)]
        [InlineData("10.0.0.0", true)]
        [InlineData("10.100.100.100", true)]
        [InlineData("10.255.255.255", true)]
        [InlineData("172.16.0.0", true)]
        [InlineData("172.23.100.100", true)]
        [InlineData("172.31.255.255", true)]
        [InlineData("192.168.0.0", true)]
        [InlineData("192.168.100.100", true)]
        [InlineData("192.168.255.255", true)]
        [InlineData("169.254.0.0", true)]
        [InlineData("169.254.100.100", true)]
        [InlineData("169.254.255.255", true)]
        public void IsPrivateIpAddressReturnsExpectedResult(string address, bool expected)
        {
            var sut = IPAddress.Parse(address);

            Assert.Equal(expected, sut.IsPrivate());
        }
    }
}