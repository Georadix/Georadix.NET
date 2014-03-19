namespace Georadix.WebApi.Testing.Tests
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

            var sut = new TestControllerFixture((InMemoryServer s) => { wasConfigCalled = true; });

            Assert.True(wasConfigCalled);
        }

        [Fact]
        public void DisposesTheInMemoryServer()
        {
            using (var sut = new TestControllerFixture(null))
            {
                // More of a placeholder, no idea how we could test this, ideas??
            }
        }

        private class TestController : ApiController
        {
        }

        private class TestControllerFixture : ApiControllerBaseFixture<TestController>
        {
            public TestControllerFixture(Action<InMemoryServer> config)
                : base(config)
            {
            }
        }
    }
}
