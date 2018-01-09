namespace Georadix.Core
{
    using System;
    using Xunit;
    using Xunit.Extensions;

    public class MathHelperFixture
    {
        [Theory]
        [InlineData(45, -75, 46, -76, 135786)]
        [InlineData(-30, 50, 10, -20, 8692982)]
        public void CalculateGreatCircleDistanceSucceeds(
            double lat1, double long1, double lat2, double long2, double expected)
        {
            Assert.Equal(expected, MathHelper.CalculateGreatCircleDistance(lat1, long1, lat2, long2), 0);
        }

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