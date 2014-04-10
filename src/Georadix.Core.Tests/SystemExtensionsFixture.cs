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
        }

        [Fact]
        public void ClipDoubleWithInvalidMinMaxThrowsArgumentOutOfRangeException()
        {
            double sut = 5.0;

            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => sut.Clip(10.0, 8.0));

            Assert.Equal("min", ex.ParamName);
            Assert.NotNull(ex.Message);
        }

        [Theory]
        [InlineData(0.1F, 5.2F, 10.3F, 5.2F)]
        [InlineData(0.1F, -10.2F, -5.3F, -5.3F)]
        public void ClipFloatReturnsExpectedResult(float sut, float min, float max, float expected)
        {
            Assert.Equal(expected, sut.Clip(min, max));
        }

        [Fact]
        public void ClipFloatWithInvalidMinMaxThrowsArgumentOutOfRangeException()
        {
            float sut = 5.0F;

            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => sut.Clip(10.0F, 8.0F));

            Assert.Equal("min", ex.ParamName);
            Assert.NotNull(ex.Message);
        }

        [Theory]
        [InlineData(0, 5, 10, 5)]
        [InlineData(0, -10, -5, -5)]
        public void ClipIntReturnsExpectedResult(int sut, int min, int max, int expected)
        {
            Assert.Equal(expected, sut.Clip(min, max));
        }

        [Fact]
        public void ClipIntWithInvalidMinMaxThrowsArgumentOutOfRangeException()
        {
            int sut = 5;

            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => sut.Clip(10, 8));

            Assert.Equal("min", ex.ParamName);
            Assert.NotNull(ex.Message);
        }

        [Theory]
        [InlineData(0, 5, 10, 5)]
        [InlineData(0, -10, -5, -5)]
        public void ClipLongReturnsExpectedResult(long sut, long min, long max, long expected)
        {
            Assert.Equal(expected, sut.Clip(min, max));
        }

        [Fact]
        public void ClipLongWithInvalidMinMaxThrowsArgumentOutOfRangeException()
        {
            long sut = 5;

            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => sut.Clip(10, 8));

            Assert.Equal("min", ex.ParamName);
            Assert.NotNull(ex.Message);
        }

        [Theory]
        [InlineData((short)0, (short)5, (short)10, (short)5)]
        [InlineData((short)0, (short)-10, (short)-5, (short)-5)]
        public void ClipShortReturnsExpectedResult(short sut, short min, short max, short expected)
        {
            Assert.Equal(expected, sut.Clip(min, max));
        }

        [Fact]
        public void ClipShortWithInvalidMinMaxThrowsArgumentOutOfRangeException()
        {
            short sut = 5;

            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => sut.Clip(10, 8));

            Assert.Equal("min", ex.ParamName);
            Assert.NotNull(ex.Message);
        }

        [Theory]
        [InlineData(0.01, 0.01, 0, true)]
        [InlineData(0.01, 0.01, 0.001, true)]
        [InlineData(0.01, 0.02, 0.1, true)]
        [InlineData(0.01, 0.02, 0.001, false)]
        public void IsEqualToDoubleReturnsExpectedResult(double sut, double value, double tolerance, bool expected)
        {
            Assert.Equal(expected, sut.IsEqualTo(value, tolerance));
        }

        [Theory]
        [InlineData(0.01F, 0.01F, 0F, true)]
        [InlineData(0.01F, 0.01F, 0.001F, true)]
        [InlineData(0.01F, 0.02F, 0.1F, true)]
        [InlineData(0.01F, 0.02F, 0.001F, false)]
        public void IsEqualToFloatReturnsExpectedResult(float sut, float value, float tolerance, bool expected)
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