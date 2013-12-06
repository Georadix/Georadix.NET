namespace Georadix.Core
{
    using System;

    /// <summary>
    /// Represents a <see cref="short"/> range.
    /// </summary>
    public class ShortRange : DiscreteRange<short>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShortRange"/> class.
        /// </summary>
        public ShortRange()
            : base(short.MaxValue, short.MinValue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShortRange"/> class with specified start and end values.
        /// </summary>
        /// <param name="start">The start value.</param>
        /// <param name="end">The end value.</param>
        public ShortRange(short start, short end)
            : base(start, end)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShortRange"/> class based on a specified range.
        /// </summary>
        /// <param name="range">The range.</param>
        public ShortRange(Range<short> range)
            : this(range.Start, range.End)
        {
        }

        /// <summary>
        /// Gets the length.
        /// </summary>
        public int Length
        {
            get { return this.IsEmpty ? 0 : (this.End - this.Start + 1); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShortRange"/> class with a specified end value.
        /// </summary>
        /// <param name="end">The end value.</param>
        /// <returns>An <see cref="ShortRange"/> instance.</returns>
        public static ShortRange EndingAt(short end)
        {
            return new ShortRange(short.MinValue, end);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShortRange"/> class with a specified start value.
        /// </summary>
        /// <param name="start">The start value.</param>
        /// <returns>An <see cref="ShortRange"/> instance.</returns>
        public static ShortRange StartingAt(short start)
        {
            return new ShortRange(start, short.MaxValue);
        }

        /// <summary>
        /// Decrements a value by 1.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The value decremented by 1.</returns>
        protected override short Decrement(short value)
        {
            return Math.Min(value, --value);
        }

        /// <summary>
        /// Increments a value by 1.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The value incremented by 1.</returns>
        protected override short Increment(short value)
        {
            return Math.Max(value, ++value);
        }
    }
}