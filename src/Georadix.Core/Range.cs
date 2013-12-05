namespace Georadix.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Abstract base class for a range of values.
    /// </summary>
    /// <typeparam name="T">The type of range.</typeparam>
    public abstract class Range<T>
        : IComparable, IComparable<Range<T>>, IEquatable<Range<T>> where T : struct, IComparable<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Range{T}"/> class with specified start and end values.
        /// </summary>
        /// <param name="start">The start value.</param>
        /// <param name="end">The end value.</param>
        protected Range(T start, T end)
        {
            this.Start = start;
            this.End = end;
        }

        /// <summary>
        /// Gets or sets the end value.
        /// </summary>
        public T End { get; set; }

        /// <summary>
        /// Gets a value indicating whether the range is empty.
        /// </summary>
        public abstract bool IsEmpty { get; }

        /// <summary>
        /// Gets or sets the start value.
        /// </summary>
        public T Start { get; set; }

        /// <summary>
        /// Combines a set of ranges into one.
        /// </summary>
        /// <typeparam name="TRange">A range class.</typeparam>
        /// <param name="ranges">The ranges.</param>
        /// <returns>A <i>TRange</i> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="ranges"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">
        /// One of the items in <paramref name="ranges"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">The ranges must be contiguous.</exception>
        public static TRange Combine<TRange>(IEnumerable<TRange> ranges) where TRange : Range<T>, new()
        {
            ranges.AssertNotNull("ranges", true);

            ranges = ranges.OrderBy(r => r);

            if (!IsContiguous(ranges))
            {
                throw new ArgumentException("The ranges must be contiguous.", "ranges");
            }

            return (ranges.Count() > 0) ?
                new TRange() { Start = ranges.First().Start, End = ranges.Last().End } : new TRange();
        }

        /// <summary>
        /// Determines whether a set of ranges contains some that overlap.
        /// </summary>
        /// <typeparam name="TRange">A range class.</typeparam>
        /// <param name="ranges">The ranges.</param>
        /// <returns>
        /// <see langword="true"/> if the set contains ranges that overlap; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="ranges"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">
        /// One of the items in <paramref name="ranges"/> is <see langword="null"/>.
        /// </exception>
        public static bool HasOverlap<TRange>(IEnumerable<TRange> ranges) where TRange : Range<T>, new()
        {
            ranges.AssertNotNull("ranges", true);

            ranges = ranges.OrderBy(r => r);

            for (var i = 0; i < ranges.Count() - 1; i++)
            {
                if (ranges.ElementAt(i).Overlaps(ranges.ElementAt(i + 1)))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether a set of ranges is contiguous.
        /// </summary>
        /// <typeparam name="TRange">A range class.</typeparam>
        /// <param name="ranges">The ranges.</param>
        /// <returns><see langword="true"/> if the set is contiguous; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="ranges"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">
        /// One of the items in <paramref name="ranges"/> is <see langword="null"/>.
        /// </exception>
        public static bool IsContiguous<TRange>(IEnumerable<TRange> ranges) where TRange : Range<T>, new()
        {
            ranges.AssertNotNull("ranges", true);

            ranges = ranges.OrderBy(r => r);

            for (var i = 0; i < ranges.Count() - 1; i++)
            {
                if (!ranges.ElementAt(i).Abuts(ranges.ElementAt(i + 1)))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether the range is adjacent to another.
        /// </summary>
        /// <typeparam name="TRange">A range class.</typeparam>
        /// <param name="range">The other range.</param>
        /// <returns>
        /// <see langword="true"/> if the range is adjacent to the other; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="range"/> is <see langword="null"/>.</exception>
        public bool Abuts<TRange>(TRange range) where TRange : Range<T>, new()
        {
            range.AssertNotNull("range");

            return !this.IsEmpty && !range.IsEmpty && !this.Overlaps(range) && this.Gap(range).IsEmpty;
        }

        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// Less than zero if this instance is less than <paramref name="obj"/>, zero if they are equal, and greater
        /// than zero if this instance is greater than <paramref name="obj"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="obj"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="obj"/> is not the same type as this instance.
        /// </exception>
        public int CompareTo(object obj)
        {
            obj.AssertNotNull("obj");

            if (!(obj is Range<T>))
            {
                throw new ArgumentException("obj is not the same type as this instance.", "obj");
            }

            return this.CompareTo(obj as Range<T>);
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// Less than zero if this object is less than <paramref name="other"/>, zero if they are equal, and greater
        /// than zero if this object is greater than <paramref name="other"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        public int CompareTo(Range<T> other)
        {
            other.AssertNotNull("other");

            return (!this.Start.Equals(other.Start)) ?
                this.Start.CompareTo(other.Start) : this.End.CompareTo(other.End);
        }

        /// <summary>
        /// Determines whether the range contains a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// <see langword="true"/> if the range contains the value; otherwise, <see langword="false"/>.
        /// </returns>
        public abstract bool Contains(T value);

        /// <summary>
        /// Determines whether the range contains another.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <returns>
        /// <see langword="true"/> if the range contains the other; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="range"/> is <see langword="null"/>.</exception>
        public abstract bool Contains(Range<T> range);

        /// <summary>
        /// Determines whether the specified <see cref="Object"/> is equal to the current <see cref="Range{T}"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare with the current <see cref="Range{T}"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is equal to the current <see cref="Range{T}"/>; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return ((obj == null) || !(obj is Range<T>)) ? false : this.Equals(obj as Range<T>);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// <see langword="true"/> if the current object is equal to <paramref name="other"/>; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public bool Equals(Range<T> other)
        {
            return (other == null) ? false : (this.Start.Equals(other.Start) && this.End.Equals(other.End));
        }

        /// <summary>
        /// Determines whether a gap exists between the range and another.
        /// </summary>
        /// <typeparam name="TRange">A range class.</typeparam>
        /// <param name="range">The other range.</param>
        /// <returns>A <i>TRange</i> instance; if there is no gap, the instance will be empty.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="range"/> is <see langword="null"/>.</exception>
        public TRange Gap<TRange>(TRange range) where TRange : Range<T>, new()
        {
            range.AssertNotNull("range");

            if (!this.Overlaps(range) && !this.IsEmpty && !range.IsEmpty)
            {
                Range<T> lower, higher;

                if (this.CompareTo(range) < 0)
                {
                    lower = this;
                    higher = range;
                }
                else
                {
                    lower = range;
                    higher = this;
                }

                return this.CreateGap<TRange>(lower.End, higher.Start);
            }

            return this.CreateEmpty<TRange>();
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current <see cref="Range{T}"/>.</returns>
        public override int GetHashCode()
        {
            return this.Start.GetHashCode() ^ this.End.GetHashCode();
        }

        /// <summary>
        /// Determines whether the range is partitioned by a set of ranges.
        /// </summary>
        /// <typeparam name="TRange">A range class.</typeparam>
        /// <param name="ranges">The ranges.</param>
        /// <returns>
        /// <see langword="true"/> if the range is partitioned by the set; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="ranges"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">
        /// One of the items in <paramref name="ranges"/> is <see langword="null"/>.
        /// </exception>
        public bool IsPartitionedBy<TRange>(IEnumerable<TRange> ranges) where TRange : Range<T>, new()
        {
            ranges.AssertNotNull("ranges", true);

            return !IsContiguous(ranges) ? false : this.Equals(Combine(ranges));
        }

        /// <summary>
        /// Determines whether the range overlaps another.
        /// </summary>
        /// <param name="range">The other range.</param>
        /// <returns>
        /// <see langword="true"/> if the range overlaps the other; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="range"/> is <see langword="null"/>.</exception>
        public abstract bool Overlaps(Range<T> range);

        /// <summary>
        /// Returns a <see cref="String"/> that represents the current <see cref="Range{T}"/>.
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current <see cref="Range{T}"/>.</returns>
        public override string ToString()
        {
            return string.Format("{0} - {1}", this.Start, this.End);
        }

        /// <summary>
        /// Creates an empty range.
        /// </summary>
        /// <typeparam name="TRange">A range class.</typeparam>
        /// <returns>A <i>TRange</i> instance.</returns>
        protected abstract TRange CreateEmpty<TRange>() where TRange : Range<T>, new();

        /// <summary>
        /// Creates a range that represents a gap between two values in other ranges.
        /// </summary>
        /// <typeparam name="TRange">A range class.</typeparam>
        /// <param name="lower">The end value of the lower range.</param>
        /// <param name="higher">The start value of the higher range.</param>
        /// <returns>A <see cref="Range{T}"/> instance.</returns>
        protected abstract TRange CreateGap<TRange>(T lower, T higher) where TRange : Range<T>, new();
    }
}