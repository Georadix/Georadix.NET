namespace Georadix.Cqs
{
    using Moq;
    using System;
    using Xunit;

    public class QueryProcessorFixture
    {
        [Fact]
        public void CreateWithNullServiceProviderThrowsArgumentNullException()
        {
            QueryProcessor sut = null;

            var ex = Assert.Throws<ArgumentNullException>(() => sut = new QueryProcessor(null));

            Assert.Equal("serviceProvider", ex.ParamName);
        }

        [Fact]
        public void ExecuteNullQueryThrowsArgumentNullException()
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);

            var sut = new QueryProcessor(serviceProviderMock.Object);

            TestQuery query = null;

            var ex = Assert.Throws<ArgumentNullException>(() => sut.Execute(query));

            Assert.Equal("query", ex.ParamName);
        }

        [Fact]
        public void ExecuteValidQueryReturnsResult()
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);
            var queryHandlerMock = new Mock<IQueryHandler<TestQuery, string>>(MockBehavior.Strict);

            var sut = new QueryProcessor(serviceProviderMock.Object);

            var query = new TestQuery();

            serviceProviderMock.Setup(sp => sp.GetService(
                typeof(IQueryHandler<TestQuery, string>))).Returns(queryHandlerMock.Object);

            queryHandlerMock.Setup(qh => qh.Handle(query)).Returns("Test");

            var result = sut.Execute(query);

            serviceProviderMock.VerifyAll();

            Assert.Equal("Test", result);
        }

        public class TestQuery : IQuery<string>
        {
        }
    }
}