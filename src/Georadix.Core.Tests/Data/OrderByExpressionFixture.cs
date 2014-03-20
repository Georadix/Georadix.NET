namespace Georadix.Core.Tests.Data
{
    using Georadix.Core.Data;
    using System;
    using Xunit;
    using Xunit.Extensions;

    public class OrderByExpressionFixture
    {
        [Fact]
        public void CreateIsProperlyInitialized()
        {
            var expression = new OrderByExpression("Property", SortDirection.Descending);

            Assert.Equal("Property", expression.PropertyName);
            Assert.Equal(SortDirection.Descending, expression.Direction);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void CreateWithInvalidPropertyNameThrowsArgumentNullException(string propertyName)
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new OrderByExpression(propertyName, SortDirection.Ascending));

            Assert.Equal("propertyName", ex.ParamName);
        }
    }
}
