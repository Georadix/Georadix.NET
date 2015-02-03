namespace Georadix.Core
{
    using System;

    /// <summary>
    /// Provides additional constants and static methods for math related functions.
    /// </summary>
    public class MathHelper
    {
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