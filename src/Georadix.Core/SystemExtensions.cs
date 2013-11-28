namespace System
{
    using Georadix.Core;

    /// <summary>
    /// Defines methods that extend system types.
    /// </summary>
    public static class SystemExtensions
    {
        #region Numeric Types

        /// <summary>
        /// Clips the number to the specified minimum and maximum values.
        /// </summary>
        /// <param name="value">The number to clip.</param>
        /// <param name="min">The minimum allowable value.</param>
        /// <param name="max">The maximum allowable value.</param>
        /// <returns>The clipped value.</returns>
        public static double Clip(this double value, double min, double max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

        /// <summary>
        /// Clips the number to the specified range.
        /// </summary>
        /// <param name="value">The number to clip.</param>
        /// <param name="range">The range.</param>
        /// <returns>The clipped value.</returns>
        public static double Clip(this double value, DoubleRange range)
        {
            return value.Clip(range.Start, range.End);
        }

        /// <summary>
        /// Clips the number to the specified minimum and maximum values.
        /// </summary>
        /// <param name="value">The number to clip.</param>
        /// <param name="min">The minimum allowable value.</param>
        /// <param name="max">The maximum allowable value.</param>
        /// <returns>The clipped value.</returns>
        public static int Clip(this int value, int min, int max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

        /// <summary>
        /// Clips the number to the specified range.
        /// </summary>
        /// <param name="value">The number to clip.</param>
        /// <param name="range">The range.</param>
        /// <returns>The clipped value.</returns>
        public static int Clip(this int value, IntRange range)
        {
            return value.Clip(range.Start, range.End);
        }

        /// <summary>
        /// Clips the number to the specified minimum and maximum values.
        /// </summary>
        /// <param name="value">The number to clip.</param>
        /// <param name="min">The minimum allowable value.</param>
        /// <param name="max">The maximum allowable value.</param>
        /// <returns>The clipped value.</returns>
        public static short Clip(this short value, short min, short max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

        /// <summary>
        /// Returns a value indicating whether this instance and a specified <see cref="Double"/> represent the same
        /// value based on a specified tolerance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="value">The value to compare to the instance.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <returns><see langword="true"/> if they are equal; otherwise, <see langword="false"/>.</returns>
        public static bool IsEqualTo(this double instance, double value, double tolerance)
        {
            return Math.Abs(instance - value) <= tolerance;
        }

        #endregion Numeric Types

        #region Event Handler Types

        /// <summary>
        /// Raises the event.
        /// </summary>
        /// <param name="handler">The event handler.</param>
        /// <param name="sender">The source of the event.</param>
        public static void Raise(this EventHandler handler, object sender)
        {
            if (handler != null)
            {
                handler(sender, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the event.
        /// </summary>
        /// <typeparam name="T">An event args class.</typeparam>
        /// <param name="handler">The event handler.</param>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        public static void Raise<T>(this EventHandler<T> handler, object sender, T e)
            where T : EventArgs
        {
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        #endregion Event Handler Types
    }
}