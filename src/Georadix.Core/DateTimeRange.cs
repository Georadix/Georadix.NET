namespace Georadix.Core
{
    using System;

    /// <summary>
    /// Represents a <see cref="DateTime"/> range.
    /// </summary>
    public sealed class DateTimeRange : ContinuousRange<DateTime>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeRange"/> class.
        /// </summary>
        public DateTimeRange()
            : this(DateTime.MaxValue, DateTime.MinValue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeRange"/> class with specified start and end
        /// values.
        /// </summary>
        /// <param name="start">The start value.</param>
        /// <param name="end">The end value.</param>
        public DateTimeRange(DateTime start, DateTime end)
            : base(start, end)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeRange"/> class based on a specified range.
        /// </summary>
        /// <param name="range">The range.</param>
        public DateTimeRange(Range<DateTime> range)
            : this(range.Start, range.End)
        {
        }

        /// <summary>
        /// Gets the length.
        /// </summary>
        public TimeSpan Length
        {
            get { return this.IsEmpty ? TimeSpan.Zero : (this.End - this.Start); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeRange"/> class with a specified end value.
        /// </summary>
        /// <param name="end">The end value.</param>
        /// <returns>An <see cref="DateTimeRange"/> instance.</returns>
        public static DateTimeRange EndingAt(DateTime end)
        {
            return new DateTimeRange(DateTime.MinValue, end);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeRange"/> class with a specified start value.
        /// </summary>
        /// <param name="start">The start value.</param>
        /// <returns>An <see cref="DateTimeRange"/> instance.</returns>
        public static DateTimeRange StartingAt(DateTime start)
        {
            return new DateTimeRange(start, DateTime.MaxValue);
        }
    }
}