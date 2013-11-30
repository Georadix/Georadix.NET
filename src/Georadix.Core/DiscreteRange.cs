namespace Georadix.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a range of discrete values.
    /// </summary>
    /// <remarks>
    /// The start and end values are inclusive.
    /// </remarks>
    /// <typeparam name="T">The type of range.</typeparam>
    public abstract class DiscreteRange<T> : Range<T>, IEnumerable<T> where T : struct, IComparable<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiscreteRange{T}"/> class with specified start and end values.
        /// </summary>
        /// <param name="start">The start value.</param>
        /// <param name="end">The end value.</param>
        protected DiscreteRange(T start, T end)
            : base(start, end)
        {
        }

        /// <summary>
        /// Gets a value that indicates whether the range is empty.
        /// </summary>
        public override bool IsEmpty
        {
            get { return this.Start.CompareTo(this.End) > 0; }
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
            return (value.CompareTo(this.Start) >= 0) && (value.CompareTo(this.End) <= 0);
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

            return this.Contains(range.Start) && this.Contains(range.End);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the range.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{T}"/> that can be used to iterate through the range.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (var value = this.Start; value.CompareTo(this.End) <= 0; value = this.Increment(value))
            {
                yield return value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
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

            return range.Contains(this.Start) || range.Contains(this.End) || this.Contains(range);
        }

        /// <summary>
        /// Creates an empty range.
        /// </summary>
        /// <typeparam name="TRange">A range class.</typeparam>
        /// <returns>A <i>TRange</i> instance.</returns>
        protected override TRange CreateEmpty<TRange>()
        {
            var instance = new TRange();
            instance.Start = this.Increment(instance.Start);
            instance.End = this.Decrement(instance.End);

            return instance;
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
            var instance = new TRange();
            instance.Start = this.Increment(lower);
            instance.End = this.Decrement(higher);

            return instance;
        }

        /// <summary>
        /// Decrements a value by one space in the number line (e.g. 1 for integers).
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The value decremented by one space in the number line.</returns>
        protected abstract T Decrement(T value);

        /// <summary>
        /// Increments a value by one space in the number line (e.g. 1 for integers).
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The value incremented by one space in the number line.</returns>
        protected abstract T Increment(T value);
    }
}