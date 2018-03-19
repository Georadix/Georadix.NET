namespace Georadix.Core.Data
{
    using System.Collections.Generic;
    using Xunit;
    using Xunit.Extensions;

    public class StringExtensionsFixture
    {
        public static IEnumerable<object[]> GetValidTryParseOrderByExpressionsScenarios
        {
            get
            {
                return new object[][]
                {
                    // Name
                    new object[]
                    {
                        "Name",
                        new OrderByExpression[]
                        {
                            new OrderByExpression("Name", SortDirection.Ascending)
                        }
                    },

                    // Name desc
                    new object[]
                    {
                        "Name desc",
                        new OrderByExpression[]
                        {
                            new OrderByExpression("Name", SortDirection.Descending)
                        }
                    },

                    // Name, Date
                    new object[]
                    {
                        "Name, Date",
                        new OrderByExpression[]
                        {
                            new OrderByExpression("Name", SortDirection.Ascending),
                            new OrderByExpression("Date", SortDirection.Ascending)
                        }
                    },

                    // Name asc, Date desc
                    new object[]
                    {
                        "Name asc, Date desc",
                        new OrderByExpression[]
                        {
                            new OrderByExpression("Name", SortDirection.Ascending),
                            new OrderByExpression("Date", SortDirection.Descending)
                        }
                    },

                    // Name desc, Date, Value desc
                    new object[]
                    {
                        "Name desc, Date, Value desc",
                        new OrderByExpression[]
                        {
                            new OrderByExpression("Name", SortDirection.Descending),
                            new OrderByExpression("Date", SortDirection.Ascending),
                            new OrderByExpression("Value", SortDirection.Descending)
                        }
                    },

                    // Name desc, Date, Value desc
                    new object[]
                    {
                        "   Name  desc   ,  Date   ,  Value    desc   ",
                        new OrderByExpression[]
                        {
                            new OrderByExpression("Name", SortDirection.Descending),
                            new OrderByExpression("Date", SortDirection.Ascending),
                            new OrderByExpression("Value", SortDirection.Descending)
                        }
                    },
                };
            }
        }

        [Theory]
        [InlineData("Name as")]
        [InlineData("N@me")]
        [InlineData("Name,,Date")]
        public void TryParseOrderByExpressionsWithInvalidSourceReturnsFalse(string orderBy)
        {
            var result = orderBy.TryParseOrderByExpressions(out OrderByExpression[] expressions);

            Assert.False(result);
        }

        [Theory]
        [MemberData("GetValidTryParseOrderByExpressionsScenarios")]
        public void TryParseOrderByExpressionsWithValidSourceReturnsExpressions(
            string orderBy, OrderByExpression[] expectedExpressions)
        {
            var result = orderBy.TryParseOrderByExpressions(out OrderByExpression[] expressions);

            Assert.True(result);

            var comparer = new DelegateEqualityComparer<OrderByExpression>(
                (OrderByExpression x, OrderByExpression y) =>
                {
                    return x.PropertyName == y.PropertyName && x.Direction == y.Direction;
                },
                (OrderByExpression obj) => { return obj.GetHashCode(); });

            Assert.Equal(expectedExpressions, expressions, comparer);
        }
    }
}
