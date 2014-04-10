namespace Georadix.WebApi
{
    using SimpleInjector;
    using System;
    using Xunit;

    public class ContextDependentExtensionsFixture
    {
        private interface ILog
        {
            Type Type { get; }
        }

        [Fact]
        public void RegisterWithContextProvidesDependencyContextOnResolveForDependentType()
        {
            var container = new Container();

            container.RegisterWithContext<ILog>(dependencyContext =>
            {
                return new Logger(dependencyContext.ImplementationType);
            });

            var controller = container.GetInstance<Controller>();

            Assert.NotNull(controller);
            Assert.NotNull(controller.Logger);
            Assert.Equal(controller.GetType(), controller.Logger.Type);
        }

        [Fact]
        public void RegisterWithContextProvidesRootContextOnResolve()
        {
            var container = new Container();

            container.RegisterWithContext<ILog>(dependencyContext =>
            {
                return new Logger(dependencyContext.ImplementationType);
            });

            var logger = container.GetInstance<ILog>();

            Assert.NotNull(logger);
            Assert.Null(logger.Type);
        }

        [Fact]
        public void RegisterWithNullContextFactoryThrowsArgumentNullException()
        {
            var container = new Container();

            var ex = Assert.Throws<ArgumentNullException>(() => container.RegisterWithContext<ILog>(null));

            Assert.Equal("contextBasedFactory", ex.ParamName);
        }

        private class Controller
        {
            private readonly ILog logger;

            public Controller(ILog logger)
            {
                this.logger = logger;
            }

            public ILog Logger
            {
                get { return this.logger; }
            }
        }

        private class Logger : ILog
        {
            private readonly Type type;

            public Logger(Type type)
            {
                this.type = type;
            }

            public Type Type
            {
                get { return this.type; }
            }
        }
    }
}