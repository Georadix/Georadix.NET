namespace Georadix.Core
{
    /// <summary>
    /// Represents a <see cref="double"/> range.
    /// </summary>
    public sealed class DoubleRange : ContinuousRange<double>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleRange"/> class.
        /// </summary>
        public DoubleRange()
            : base(double.MaxValue, double.MinValue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleRange"/> class with specified start and end values.
        /// </summary>
        /// <param name="start">The start value.</param>
        /// <param name="end">The end value.</param>
        public DoubleRange(double start, double end)
            : base(start, end)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleRange"/> class based on a specified range.
        /// </summary>
        /// <param name="range">The range.</param>
        public DoubleRange(Range<double> range)
            : this(range.Start, range.End)
        {
        }

        /// <summary>
        /// Gets the length.
        /// </summary>
        public double Length
        {
            get { return this.IsEmpty ? 0 : (this.End - this.Start); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleRange"/> class with a specified end value.
        /// </summary>
        /// <param name="end">The end value.</param>
        /// <returns>An <see cref="DoubleRange"/> instance.</returns>
        public static DoubleRange EndingAt(double end)
        {
            return new DoubleRange(double.MinValue, end);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleRange"/> class with a specified start value.
        /// </summary>
        /// <param name="start">The start value.</param>
        /// <returns>An <see cref="DoubleRange"/> instance.</returns>
        public static DoubleRange StartingAt(double start)
        {
            return new DoubleRange(start, double.MaxValue);
        }
    }
}