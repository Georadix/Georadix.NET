namespace Georadix.WebApi.Testing
{
    using System;
    using System.Web.Http;
    using Xunit;

    public class ApiControllerBaseFixtureFixture
    {
        [Fact]
        public void CreateWithConfigurationCallbackGetsCalled()
        {
            bool wasConfigCalled = false;

            var sut = new TestControllerFixture((InMemoryServer s) => { wasConfigCalled = true; }, () => { });

            Assert.True(wasConfigCalled);
        }

        [Fact]
        public void DisposesTheInstanceProperly()
        {
            var wasDisposed = false;

            using (var sut = new TestControllerFixture(null, () => { wasDisposed = true; }))
            {
                // Placeholder until we figure out how to properly test that it disposes dependencies properly.
            }

            Assert.True(wasDisposed);
        }

        private class TestController : ApiController
        {
        }

        private class TestControllerFixture : ApiControllerBaseFixture<TestController>
        {
            private readonly Action disposedCallback;

            public TestControllerFixture(Action<InMemoryServer> config, Action disposedCallback)
                : base(config)
            {
                this.disposedCallback = disposedCallback;
            }

            protected override void Dispose(bool disposing)
            {
                this.disposedCallback();

                base.Dispose(disposing);
            }
        }
    }
}
