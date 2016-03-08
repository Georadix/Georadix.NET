namespace Georadix.WebApi
{
    using Georadix.WebApi.Filters;
    using Georadix.WebApi.Testing;
    using log4net;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
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

                Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
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
                var response = await server.Client.GetAsync("test/nested-exception");

                Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
                Assert.True(this.loggerMocks.ContainsKey(typeof(LogExceptionFilterAttributeFixtureController)));

                this.loggerMocks[typeof(LogExceptionFilterAttributeFixtureController)].Verify(l => l.Error(
                    LogExceptionFilterAttributeFixtureController.NestedException.GetBaseException().Message,
                    LogExceptionFilterAttributeFixtureController.NestedException.GetBaseException()));
            }
        }

        [Fact]
        public async Task ExceptionWithDataFromControllerLoggedAsError()
        {
            using (var server = this.CreateServer())
            {
                var response = await server.Client.GetAsync("test/exception-with-data");

                Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
                Assert.True(this.loggerMocks.ContainsKey(typeof(LogExceptionFilterAttributeFixtureController)));

                var exception = LogExceptionFilterAttributeFixtureController.ExceptionWithData;
                var keyValueStrings = new List<string>();
                foreach (var key in exception.Data.Keys)
                {
                    keyValueStrings.Add(string.Format("{0}: {1}", key, exception.Data[key]));
                }

                this.loggerMocks[typeof(LogExceptionFilterAttributeFixtureController)].Verify(l => l.Error(
                    It.Is<string>(m => m.StartsWith(exception.Message) && keyValueStrings.All(s => m.Contains(s))),
                    LogExceptionFilterAttributeFixtureController.ExceptionWithData));
            }
        }

        [Fact]
        public async Task HttpResponseExceptionFromControllerIsReturnedAndNotLogged()
        {
            using (var server = this.CreateServer())
            {
                var response = await server.Client.GetAsync("test/http-response-exception");

                Assert.Equal(
                    LogExceptionFilterAttributeFixtureController.HttpResponseException.Response.StatusCode,
                    response.StatusCode);
                Assert.False(this.loggerMocks.ContainsKey(typeof(LogExceptionFilterAttributeFixtureController)));
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
        public static readonly InvalidOperationException ExceptionWithData;
        public static readonly HttpResponseException HttpResponseException;
        public static readonly InvalidOperationException NestedException;

        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations",
            Justification = "This is simply for making testing raised exceptions easier.")]
        static LogExceptionFilterAttributeFixtureController()
        {
            Exception = new InvalidOperationException("Error Message");
            NestedException = new InvalidOperationException(
                "Error Message", new InvalidOperationException("Inner Error Message"));
            HttpResponseException = new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            ExceptionWithData = new InvalidOperationException("Error Message With Data");
            ExceptionWithData.Data.Add("First", "Value 1");
            ExceptionWithData.Data.Add("Second", "Value 2");
        }

        [Route("exception")]
        public void GetException()
        {
            throw Exception;
        }

        [Route("exception-with-data")]
        public void GetExceptionWithData()
        {
            throw ExceptionWithData;
        }

        [Route("http-response-exception")]
        public void GetHttpResponseException()
        {
            throw HttpResponseException;
        }

        [Route("nested-exception")]
        public void GetNestedException()
        {
            throw NestedException;
        }
    }
}