namespace Georadix.WebApi.Testing
{
    using Xunit;
    using Xunit.Extensions;

    public class InMemoryServerFixture
    {
        [Theory]
        [InlineData(false, "http")]
        [InlineData(true, "https")]
        public void ConstructorReturnsInitializedInstance(bool useHttps, string expectedScheme)
        {
            var server = new InMemoryServer(useHttps);

            Assert.NotNull(server.Client);
            Assert.NotNull(server.Configuration);
            Assert.Equal(expectedScheme, server.Client.BaseAddress.Scheme);
        }
    }
}