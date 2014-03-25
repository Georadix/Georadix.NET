namespace Georadix.Core
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Xunit;
    using Xunit.Extensions;

    public class SystemExtensionsFixture
    {
        #region Type

        [Fact]
        public void GetGenericMethodReturnsMethodInfo()
        {
            var methodInfo = typeof(Queryable).GetGenericMethod(
                "OrderBy", typeof(IQueryable<string>).GetGenericTypeDefinition(), typeof(Expression<>));

            Assert.NotNull(methodInfo);
            Assert.Equal("OrderBy", methodInfo.Name);
        }

        #endregion

        #region Numeric Types

        [Theory]
        [InlineData(0.1, 5.2, 10.3, 5.2)]
        [InlineData(0.1, -10.2, -5.3, -5.3)]
        public void ClipDoubleReturnsExpectedResult(double sut, double min, double max, double expected)
        {
            Assert.Equal(expected, sut.Clip(min, max));

            var range = new DoubleRange(min, max);

            Assert.Equal(expected, sut.Clip(range));
        }

        [Theory]
        [InlineData(0, 5, 10, 5)]
        [InlineData(0, -10, -5, -5)]
        public void ClipIntegerReturnsExpectedResult(int sut, int min, int max, int expected)
        {
            Assert.Equal(expected, sut.Clip(min, max));

            var range = new IntRange(min, max);

            Assert.Equal(expected, sut.Clip(range));
        }

        [Theory]
        [InlineData((short)0, (short)5, (short)10, (short)5)]
        [InlineData((short)0, (short)-10, (short)-5, (short)-5)]
        public void ClipShortReturnsExpectedResult(short sut, short min, short max, short expected)
        {
            Assert.Equal(expected, sut.Clip(min, max));

            var range = new ShortRange(min, max);

            Assert.Equal(expected, sut.Clip(range));
        }

        [Theory]
        [InlineData(0.01, 0.01, 0, true)]
        [InlineData(0.01, 0.01, 0.001, true)]
        [InlineData(0.01, 0.02, 0.1, true)]
        [InlineData(0.01, 0.02, 0.001, false)]
        public void IsEqualToReturnsExpectedResult(double sut, double value, double tolerance, bool expected)
        {
            Assert.Equal(expected, sut.IsEqualTo(value, tolerance));
        }

        #endregion Numeric Types

        #region EventHandler Types

        [Fact]
        public void RaiseEventHandlerSucceeds()
        {
            object sender = null;

            var handler = new EventHandler((obj, e) =>
            {
                sender = obj;
                Assert.Equal(EventArgs.Empty, e);
            });

            var expected = new object();
            handler.Raise(expected);

            Assert.Equal(expected, sender);
        }

        [Fact]
        public void RaiseGenericEventHandlerSucceeds()
        {
            object sender = null;
            var eventArgs = new UnhandledExceptionEventArgs(null, true);

            var handler = new EventHandler<UnhandledExceptionEventArgs>((obj, e) =>
            {
                sender = obj;
                Assert.Equal(eventArgs, e);
            });

            var expected = new object();
            handler.Raise(expected, eventArgs);

            Assert.Equal(expected, sender);
        }

        #endregion EventHandler Types
    }
}