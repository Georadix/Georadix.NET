namespace Georadix.Core
{
    using System;

    /// <summary>
    /// Represents a <see cref="DateTimeOffset"/> range.
    /// </summary>
    public sealed class DateTimeOffsetRange : ContinuousRange<DateTimeOffset>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeOffsetRange"/> class.
        /// </summary>
        public DateTimeOffsetRange()
            : this(DateTimeOffset.MaxValue, DateTimeOffset.MinValue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeOffsetRange"/> class with specified start and end
        /// values.
        /// </summary>
        /// <param name="start">The start value.</param>
        /// <param name="end">The end value.</param>
        public DateTimeOffsetRange(DateTimeOffset start, DateTimeOffset end)
            : base(start, end)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeOffsetRange"/> class based on a specified range.
        /// </summary>
        /// <param name="range">The range.</param>
        public DateTimeOffsetRange(Range<DateTimeOffset> range)
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
        /// Initializes a new instance of the <see cref="DateTimeOffsetRange"/> class with a specified end value.
        /// </summary>
        /// <param name="end">The end value.</param>
        /// <returns>An <see cref="DateTimeOffsetRange"/> instance.</returns>
        public static DateTimeOffsetRange EndingAt(DateTimeOffset end)
        {
            return new DateTimeOffsetRange(DateTimeOffset.MinValue, end);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeOffsetRange"/> class with a specified start value.
        /// </summary>
        /// <param name="start">The start value.</param>
        /// <returns>An <see cref="DateTimeOffsetRange"/> instance.</returns>
        public static DateTimeOffsetRange StartingAt(DateTimeOffset start)
        {
            return new DateTimeOffsetRange(start, DateTimeOffset.MaxValue);
        }
    }
}