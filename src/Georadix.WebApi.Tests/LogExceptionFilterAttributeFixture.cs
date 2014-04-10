namespace Georadix.WebApi
{
    using Georadix.WebApi.Filters;
    using Georadix.WebApi.Testing;
    using log4net;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Xunit;

    public class LogExceptionFilterAttributeFixture
    {
        private readonly Dictionary<Type, Mock<ILog>> loggerMocks = new Dictionary<Type, Mock<ILog>>();

        [Fact]
        public void ConstructorWithNullLoggerFactoryThrowsArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new LogExceptionFilterAttribute(null));

            Assert.Equal("loggerFactory", ex.ParamName);
        }

        [Fact]
        public async Task ExceptionFromControllerLoggedAsError()
        {
            using (var server = this.CreateServer())
            {
                var response = await server.Client.GetAsync("test/exception");

                Assert.True(this.loggerMocks.ContainsKey(typeof(LogExceptionFilterAttributeFixtureController)));

                this.loggerMocks[typeof(LogExceptionFilterAttributeFixtureController)].Verify(l => l.Error(
                    LogExceptionFilterAttributeFixtureController.Exception.Message,
                    LogExceptionFilterAttributeFixtureController.Exception));
            }
        }

        [Fact]
        public async Task ExceptionFromControllerLogsInnerException()
        {
            using (var server = this.CreateServer())
            {
                var response = await server.Client.GetAsync("test/nestedxception");

                Assert.True(this.loggerMocks.ContainsKey(typeof(LogExceptionFilterAttributeFixtureController)));

                this.loggerMocks[typeof(LogExceptionFilterAttributeFixtureController)].Verify(l => l.Error(
                    LogExceptionFilterAttributeFixtureController.NestedException.GetBaseException().Message,
                    LogExceptionFilterAttributeFixtureController.NestedException.GetBaseException()));
            }
        }

        private InMemoryServer CreateServer()
        {
            var server = new InMemoryServer();

            server.Configuration.Filters.Add(new LogExceptionFilterAttribute((t) =>
                {
                    if (!this.loggerMocks.ContainsKey(t))
                    {
                        this.loggerMocks.Add(t, new Mock<ILog>());
                    }

                    return this.loggerMocks[t].Object;
                }));

            server.Configuration.MapHttpAttributeRoutes();

            return server;
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