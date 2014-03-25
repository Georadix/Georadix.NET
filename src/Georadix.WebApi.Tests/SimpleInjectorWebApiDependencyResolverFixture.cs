namespace Georadix.WebApi
{
    using SimpleInjector;
    using System;
    using Xunit;

    public class SimpleInjectorWebApiDependencyResolverFixture
    {
        [Fact]
        public void CreateWithNullContainerThrowsArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new SimpleInjectorWebApiDependencyResolver(null));

            Assert.Equal("container", ex.ParamName);

            var sut = new SimpleInjectorWebApiDependencyResolver(new Container());
        }

        [Fact]
        public void BeginScopeReturnsItself()
        {
            var sut = new SimpleInjectorWebApiDependencyResolver(new Container());

            Assert.Same(sut, sut.BeginScope());
        }

        [Fact]
        public void GetServiceGoesThroughTheContainerAndReturnsTheService()
        {
            var container = new Container();

            var sut = new SimpleInjectorWebApiDependencyResolver(container);

            container.RegisterSingle(this.GetType(), this);

            var service = sut.GetService(this.GetType());

            Assert.Same(this, service);
        }

        [Fact]
        public void GetServicesGoesThroughTheContainerAndReturnsServices()
        {
            var container = new Container();

            var sut = new SimpleInjectorWebApiDependencyResolver(container);

            var services = sut.GetServices(this.GetType());

            Assert.Empty(services);
        }

        [Fact]
        public void DisposeDoesNothing()
        {
            var sut = new SimpleInjectorWebApiDependencyResolver(new Container());

            sut.Dispose();
        }
    }
}