namespace Georadix.Core.Tests
{
    using System;
    using Xunit;
    using Xunit.Extensions;

    public class MathHelperFixture
    {
        [Theory]
        [InlineData(-180, -Math.PI)]
        [InlineData(-90, -Math.PI / 2)]
        [InlineData(0, 0)]
        [InlineData(90, Math.PI / 2)]
        [InlineData(180, Math.PI)]
        public void DegreesToRadiansSucceeds(int degrees, double expected)
        {
            Assert.Equal(expected, MathHelper.DegreesToRadians(degrees));
        }
    }
}