namespace Georadix.Core
{
    using System;

    /// <summary>
    /// Represents a range of continuous values.
    /// </summary>
    /// <remarks>
    /// The start value is inclusive and the end value is exclusive.
    /// </remarks>
    /// <typeparam name="T">The type of range.</typeparam>
    public abstract class ContinuousRange<T> : Range<T> where T : struct, IComparable<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContinuousRange{T}"/> class with specified start and end
        /// values.
        /// </summary>
        /// <param name="start">The start value.</param>
        /// <param name="end">The end value.</param>
        protected ContinuousRange(T start, T end)
            : base(start, end)
        {
        }

        /// <summary>
        /// Gets a value that indicates whether the range is empty.
        /// </summary>
        public override bool IsEmpty
        {
            get { return this.Start.CompareTo(this.End) >= 0; }
        }

        /// <summary>
        /// Determines whether the range contains a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// <see langword="true"/> if the range contains the value; otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Contains(T value)
        {
            return (value.CompareTo(this.Start) >= 0) && (value.CompareTo(this.End) < 0);
        }

        /// <summary>
        /// Determines whether the range contains another.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <returns>
        /// <see langword="true"/> if the range contains the other; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="range"/> is <see langword="null"/>.</exception>
        public override bool Contains(Range<T> range)
        {
            range.AssertNotNull("range");

            return this.Contains(range.Start) &&
                (range.End.CompareTo(this.Start) > 0) &&
                (range.End.CompareTo(this.End) <= 0);
        }

        /// <summary>
        /// Determines whether the range overlaps another.
        /// </summary>
        /// <param name="range">The other range.</param>
        /// <returns>
        /// <see langword="true"/> if the range overlaps the other; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="range"/> is <see langword="null"/>.</exception>
        public override bool Overlaps(Range<T> range)
        {
            range.AssertNotNull("range");

            return range.Contains(this.Start) ||
                ((this.End.CompareTo(range.Start) > 0) && (this.End.CompareTo(range.End) <= 0)) ||
                this.Contains(range);
        }

        /// <summary>
        /// Creates an empty range.
        /// </summary>
        /// <typeparam name="TRange">A range class.</typeparam>
        /// <returns>A <i>TRange</i> instance.</returns>
        protected override TRange CreateEmpty<TRange>()
        {
            return new TRange();
        }

        /// <summary>
        /// Creates a range that represents a gap between two values in other ranges.
        /// </summary>
        /// <typeparam name="TRange">A range class.</typeparam>
        /// <param name="lower">The end value of the lower range.</param>
        /// <param name="higher">The start value of the higher range.</param>
        /// <returns>A <see cref="Range{T}"/> instance.</returns>
        protected override TRange CreateGap<TRange>(T lower, T higher)
        {
            return new TRange() { Start = lower, End = higher };
        }
    }
}