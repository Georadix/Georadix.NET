namespace Georadix.Services.Tests
{
    using Georadix.Data;
    using Moq;
    using System;
    using Xunit;

    public class QueryServiceFixture
    {
        [Fact]
        public void CreateWithNullServiceProviderThrowsArgumentNullException()
        {
            QueryService service = null;

            var ex = Assert.Throws<ArgumentNullException>(() => service = new QueryService(null));

            Assert.Equal("serviceProvider", ex.ParamName);
        }

        [Fact]
        public void ExecuteNullQueryThrowsArgumentNullException()
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);

            var service = new QueryService(serviceProviderMock.Object);

            TestQuery query = null;

            var ex = Assert.Throws<ArgumentNullException>(() => service.Execute(query));

            Assert.Equal("query", ex.ParamName);
        }

        [Fact]
        public void ExecuteValidQueryReturnsResult()
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);
            var queryHandlerMock = new Mock<IQueryHandler<TestQuery, string>>(MockBehavior.Strict);

            var service = new QueryService(serviceProviderMock.Object);

            var query = new TestQuery();

            serviceProviderMock.Setup(sp => sp.GetService(
                typeof(IQueryHandler<TestQuery, string>))).Returns(queryHandlerMock.Object);

            queryHandlerMock.Setup(qh => qh.Handle(query)).Returns("Test");

            var result = service.Execute(query);

            serviceProviderMock.VerifyAll();

            Assert.Equal("Test", result);
        }

        public class TestQuery : IQuery<string>
        {
        }
    }
}