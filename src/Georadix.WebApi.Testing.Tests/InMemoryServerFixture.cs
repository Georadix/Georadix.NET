namespace Georadix.WebApi.Testing.Tests
{
    using SimpleInjector;
    using System;
    using Xunit;

    public class InMemoryServerFixture
    {
        [Fact]
        public void CreateWithNullContainerThrowsArgumentNullException()
        {
            InMemoryServer server;

            var ex = Assert.Throws<ArgumentNullException>(() => server = new InMemoryServer(null));

            Assert.Equal("container", ex.ParamName);
        }

        [Fact]
        public void CreateWithConfigurationCallbackGetsCalled()
        {
            bool wasConfigCalled = false;

            var server = new InMemoryServer(
                new Container(), 
                (InMemoryServer s) => 
                {
                    Assert.NotNull(s);
                    wasConfigCalled = true;
                });

            Assert.True(wasConfigCalled);
        }

        [Fact]
        public void CreateIsProperlyInitialized()
        {
            var container = new Container();
            var server = new InMemoryServer(container);

            Assert.Same(container, server.Container);
            Assert.NotNull(server.Client);
            Assert.NotNull(server.Configuration);
            Assert.NotNull(server.LoggerFactory);
            Assert.NotNull(server.MockLoggers);
        }
    }
}
