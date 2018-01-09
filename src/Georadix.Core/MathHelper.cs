namespace Georadix.Core
{
    using System;

    /// <summary>
    /// Provides additional constants and static methods for math related functions.
    /// </summary>
    public class MathHelper
    {
        /// <summary>
        /// Calculates the great circle distance, in meters, between two latitude/longitude points.
        /// </summary>
        /// <param name="lat1">The latitude of the first point.</param>
        /// <param name="long1">The longitude of the first point.</param>
        /// <param name="lat2">The latitude of the second point.</param>
        /// <param name="long2">The longitude of the second point.</param>
        /// <returns>The value in meters.</returns>
        public static double CalculateGreatCircleDistance(double lat1, double long1, double lat2, double long2)
        {
            var earthRadius = 6371e3;
            var deltaLat = DegreesToRadians(lat2 - lat1);
            var deltaLong = DegreesToRadians(long2 - long1);

            var a = (Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2)) +
                    (Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                    Math.Sin(deltaLong / 2) * Math.Sin(deltaLong / 2));
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return earthRadius * c;
        }

        /// <summary>
        /// Converts a value in degrees to its equivalent in radians.
        /// </summary>
        /// <param name="degrees">The value in degrees.</param>
        /// <returns>The value in radians.</returns>
        public static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
    }
}