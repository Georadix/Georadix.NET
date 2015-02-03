namespace Georadix.WebApi.Testing
{
    using Xunit;

    public class InMemoryServerFixture
    {
        [Fact]
        public void ConstructorReturnsInitializedInstance()
        {
            var server = new InMemoryServer();

            Assert.NotNull(server.Client);
            Assert.NotNull(server.Configuration);
        }
    }
}
