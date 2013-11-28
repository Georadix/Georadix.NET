namespace Georadix.Core
{
    using System;

    /// <summary>
    /// Represents an <see cref="int"/> range.
    /// </summary>
    public sealed class IntRange : DiscreteRange<int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntRange"/> class.
        /// </summary>
        public IntRange()
            : base(int.MaxValue, int.MinValue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntRange"/> class with specified start and end values.
        /// </summary>
        /// <param name="start">The start value.</param>
        /// <param name="end">The end value.</param>
        public IntRange(int start, int end)
            : base(start, end)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntRange"/> class based on a specified range.
        /// </summary>
        /// <param name="range">The range.</param>
        public IntRange(Range<int> range)
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
        /// Initializes a new instance of the <see cref="IntRange"/> class with a specified end value.
        /// </summary>
        /// <param name="end">The end value.</param>
        /// <returns>An <see cref="IntRange"/> instance.</returns>
        public static IntRange EndingAt(int end)
        {
            return new IntRange(int.MinValue, end);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntRange"/> class with a specified start value.
        /// </summary>
        /// <param name="start">The start value.</param>
        /// <returns>An <see cref="IntRange"/> instance.</returns>
        public static IntRange StartingAt(int start)
        {
            return new IntRange(start, int.MaxValue);
        }

        /// <summary>
        /// Decrements a value by 1.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The value decremented by 1.</returns>
        protected override int Decrement(int value)
        {
            return Math.Min(value, value - 1);
        }

        /// <summary>
        /// Increments a value by 1.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The value incremented by 1.</returns>
        protected override int Increment(int value)
        {
            return Math.Max(value, value + 1);
        }
    }
}