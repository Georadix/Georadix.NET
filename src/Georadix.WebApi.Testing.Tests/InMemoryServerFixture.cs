namespace Georadix.WebApi.Testing
{
    using System;
    using Xunit;
    using Xunit.Extensions;

    public class InMemoryServerFixture
    {
        [Theory]
        [InlineData(false, "http")]
        [InlineData(true, "https")]
        public void ConstructorReturnsInitializedInstance(bool useHttps, string expectedScheme)
        {
            var sut = new InMemoryServer(useHttps);

            Assert.NotNull(sut.Client);
            Assert.NotNull(sut.Configuration);
            Assert.Equal(expectedScheme, sut.Client.BaseAddress.Scheme);
        }

        [Fact]
        public void ConstructorWithBaseAddressReturnsInitializedInstance()
        {
            var baseAddress = new Uri("http://my.server.com");

            var sut = new InMemoryServer(baseAddress);

            Assert.Equal(baseAddress, sut.Client.BaseAddress);
        }

        [Fact]
        public void ConstructorWithNullBaseAddressThrowsArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new InMemoryServer(null));

            Assert.Equal("baseAddress", ex.ParamName);
        }
    }
}