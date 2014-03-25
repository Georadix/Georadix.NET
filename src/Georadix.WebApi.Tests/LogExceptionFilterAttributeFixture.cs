namespace Georadix.WebApi
{
    using Georadix.WebApi.Filters;
    using Georadix.WebApi.Testing;
    using SimpleInjector;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Xunit;

    public class LogExceptionFilterAttributeFixture
    {
        [Fact]
        public void CreateWithNullLoggerFactoryThrowsArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new LogExceptionFilterAttribute(null));

            Assert.Equal("loggerFactory", ex.ParamName);
        }

        [Fact]
        public async Task ExceptionFromControllerLoggedAsError()
        {
            using (var server = new InMemoryServer(new Container(), this.ConfigureServer))
            {
                var response = await server.Client.GetAsync("test/exception");

                Assert.True(server.MockLoggers.ContainsKey(typeof(LogExceptionFilterAttributeFixtureController)));

                server.MockLoggers[typeof(LogExceptionFilterAttributeFixtureController)].Verify(l => l.Error(
                    LogExceptionFilterAttributeFixtureController.Exception.Message,
                    LogExceptionFilterAttributeFixtureController.Exception));
            }
        }

        [Fact]
        public async Task ExceptionFromControllerLogsInnerException()
        {
            using (var server = new InMemoryServer(new Container(), this.ConfigureServer))
            {
                server.Container.Register<LogExceptionFilterAttributeFixtureController>();

                var response = await server.Client.GetAsync("test/nestedxception");

                Assert.True(server.MockLoggers.ContainsKey(typeof(LogExceptionFilterAttributeFixtureController)));

                server.MockLoggers[typeof(LogExceptionFilterAttributeFixtureController)].Verify(l => l.Error(
                    LogExceptionFilterAttributeFixtureController.NestedException.GetBaseException().Message,
                    LogExceptionFilterAttributeFixtureController.NestedException.GetBaseException()));
            }
        }

        private void ConfigureServer(InMemoryServer server)
        {
            server.Configuration.Filters.Add(new LogExceptionFilterAttribute(server.LoggerFactory));

            server.Configuration.MapHttpAttributeRoutes();
        }
    }

    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Embedding this controller class inside the fixture causes routing to its methods to fail.")]
    [RoutePrefix("test")]
    public class LogExceptionFilterAttributeFixtureController : ApiController
    {
        public static readonly InvalidOperationException Exception;

        public static readonly InvalidOperationException NestedException;

        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations",
            Justification = "This is simply for making testing raised exceptions easier.")]
        static LogExceptionFilterAttributeFixtureController()
        {
            Exception = new InvalidOperationException("Error Message");
            NestedException = new InvalidOperationException(
                "Error Message", new InvalidOperationException("Inner Error Message"));
        }

        [Route("exception")]
        public string GetException()
        {
            throw Exception;
        }

        [Route("nestedxception")]
        public void GetNestedException()
        {
            throw NestedException;
        }
    }
}