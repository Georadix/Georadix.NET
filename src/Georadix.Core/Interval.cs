namespace Georadix.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Enumerates the different types of interval endpoints.
    /// </summary>
    public enum EndpointType
    {
        /// <summary>
        /// Open endpoint.
        /// </summary>
        Open,

        /// <summary>
        /// Closed endpoint.
        /// </summary>
        Closed,

        /// <summary>
        /// Unbounded endpoint.
        /// </summary>
        Unbounded
    }

    /// <summary>
    /// Represents a set of values.
    /// </summary>
    /// <typeparam name="T">The type of value in the interval.</typeparam>
    public class Interval<T>
        : IComparable, IComparable<Interval<T>>, IEquatable<Interval<T>> where T : struct, IComparable<T>
    {
        private int endpoints;
        private T? left;
        private T? right;

        /// <summary>
        /// Initializes a new instance of the <see cref="Interval{T}"/> class.
        /// </summary>
        /// <param name="left">The left endpoint.</param>
        /// <param name="leftType">The left endpoint type.</param>
        /// <param name="right">The right endpoint.</param>
        /// <param name="rightType">The right endpoint type.</param>
        /// <exception cref="ArgumentException">The interval is empty.</exception>
        private Interval(T? left, EndpointType leftType, T? right, EndpointType rightType)
        {
            CloseEndpoint(ref left, ref leftType, 1);
            CloseEndpoint(ref right, ref rightType, -1);

            if ((left.HasValue && right.HasValue) &&
                IsEmpty(left.Value, leftType == EndpointType.Closed, right.Value, rightType == EndpointType.Closed))
            {
                throw new ArgumentException("The interval cannot be empty.");
            }

            this.left = left;
            this.right = right;
            this.endpoints =
                (leftType == EndpointType.Closed ? 0x1 : 0) | (rightType == EndpointType.Closed ? 0x2 : 0);
        }

        /// <summary>
        /// Gets the left endpoint.
        /// </summary>
        public T? Left
        {
            get { return this.left; }
        }

        /// <summary>
        /// Gets the left endpoint type.
        /// </summary>
        public EndpointType LeftType
        {
            get
            {
                return this.Left.HasValue ?
                    ((this.endpoints & 0x1) != 0 ? EndpointType.Closed : EndpointType.Open) : EndpointType.Unbounded;
            }
        }

        /// <summary>
        /// Gets the right endpoint.
        /// </summary>
        public T? Right
        {
            get { return this.right; }
        }

        /// <summary>
        /// Gets the right endpoint type.
        /// </summary>
        public EndpointType RightType
        {
            get
            {
                return this.Right.HasValue ?
                    ((this.endpoints & 0x2) != 0 ? EndpointType.Closed : EndpointType.Open) : EndpointType.Unbounded;
            }
        }

        /// <summary>
        /// Initializes a new bounded instance of the <see cref="Interval{T}"/> class.
        /// </summary>
        /// <param name="left">The left endpoint.</param>
        /// <param name="isLeftClosed">A value indicating whether the left endpoint is closed.</param>
        /// <param name="right">The right.</param>
        /// <param name="isRightClosed">A value indicating whether the right endpoint is closed.</param>
        /// <returns>An <see cref="Interval{T}"/> instance.</returns>
        public static Interval<T> Bounded(T left, bool isLeftClosed, T right, bool isRightClosed)
        {
            return new Interval<T>(
                left,
                isLeftClosed ? EndpointType.Closed : EndpointType.Open,
                right,
                isRightClosed ? EndpointType.Closed : EndpointType.Open);
        }

        /// <summary>
        /// Combines a set of intervals into one.
        /// </summary>
        /// <param name="intervals">The intervals.</param>
        /// <returns>An <see cref="Interval{T}"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="intervals"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">The intervals are not contiguous.</exception>
        public static Interval<T> Combine(IEnumerable<Interval<T>> intervals)
        {
            intervals.AssertNotNull("intervals");

            if (!intervals.Any())
            {
                return null;
            }

            intervals = intervals.OrderBy(i => i);

            if (!IsContiguous(intervals))
            {
                throw new ArgumentException("The intervals must be contiguous.", "intervals");
            }

            var first = intervals.First();
            var last = intervals.Last();

            if ((first.LeftType == EndpointType.Unbounded) && (last.RightType == EndpointType.Unbounded))
            {
                return Interval<T>.Unbounded();
            }
            else if (first.LeftType == EndpointType.Unbounded)
            {
                return Interval<T>.RightBounded(last.Right.Value, last.RightType == EndpointType.Closed);
            }
            else if (last.RightType == EndpointType.Unbounded)
            {
                return Interval<T>.LeftBounded(first.Left.Value, first.LeftType == EndpointType.Closed);
            }
            else
            {
                return Interval<T>.Bounded(
                    first.Left.Value,
                    first.LeftType == EndpointType.Closed,
                    last.Right.Value,
                    last.RightType == EndpointType.Closed);
            }
        }

        /// <summary>
        /// Determines whether a set of intervals contains some that overlap.
        /// </summary>
        /// <param name="intervals">The intervals.</param>
        /// <returns>
        /// <see langword="true"/> if the set contains intervals that overlap; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="intervals"/> is <see langword="null"/>.</exception>
        public static bool HasOverlap(IEnumerable<Interval<T>> intervals)
        {
            intervals.AssertNotNull("intervals");

            intervals = intervals.OrderBy(i => i);

            for (var i = 0; i < intervals.Count() - 1; i++)
            {
                if (intervals.ElementAt(i) == null)
                {
                    break;
                }

                if (intervals.ElementAt(i).Overlaps(intervals.ElementAt(i + 1)))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether a set of intervals is contiguous.
        /// </summary>
        /// <param name="intervals">The intervals.</param>
        /// <returns><see langword="true"/> if the set is contiguous; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="intervals"/> is <see langword="null"/>.</exception>
        public static bool IsContiguous(IEnumerable<Interval<T>> intervals)
        {
            intervals.AssertNotNull("intervals");

            intervals = intervals.OrderBy(i => i);

            for (var i = 0; i < intervals.Count() - 1; i++)
            {
                if ((intervals.ElementAt(i) == null) || !intervals.ElementAt(i).Abuts(intervals.ElementAt(i + 1)))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether the specified interval is empty.
        /// </summary>
        /// <param name="left">The left endpoint.</param>
        /// <param name="isLeftClosed">A value indicating whether the left endpoint is closed.</param>
        /// <param name="right">The right endpoint.</param>
        /// <param name="isRightClosed">A value indicating whether the right endpoint is closed.</param>
        /// <returns>
        /// <see langword="true"/> if the interval is empty (i.e. [b,a] (a,a) [a,a) (a,a]); otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public static bool IsEmpty(T left, bool isLeftClosed, T right, bool isRightClosed)
        {
            T? leftEndpoint = left;
            EndpointType leftType = isLeftClosed ? EndpointType.Closed : EndpointType.Open;
            T? rightEndpoint = right;
            EndpointType rightType = isRightClosed ? EndpointType.Closed : EndpointType.Open;

            CloseEndpoint(ref leftEndpoint, ref leftType, 1);
            CloseEndpoint(ref rightEndpoint, ref rightType, -1);

            return
                (leftEndpoint.Value.CompareTo(rightEndpoint.Value) > 0) ||
                (leftEndpoint.Equals(rightEndpoint) &&
                    ((leftType != EndpointType.Closed) || (rightType != EndpointType.Closed)));
        }

        /// <summary>
        /// Initializes a new left-bounded instance of the <see cref="Interval{T}"/> class.
        /// </summary>
        /// <param name="left">The left endpoint.</param>
        /// <param name="isLeftClosed">A value indicating whether the left endpoint is closed.</param>
        /// <returns>An <see cref="Interval{T}"/> instance.</returns>
        public static Interval<T> LeftBounded(T left, bool isLeftClosed)
        {
            return new Interval<T>(
                left, isLeftClosed ? EndpointType.Closed : EndpointType.Open, null, EndpointType.Unbounded);
        }

        /// <summary>
        /// Returns whether two intervals are not equal.
        /// </summary>
        /// <param name="a">The first interval.</param>
        /// <param name="b">The second interval.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="a"/> is not equal to <paramref name="b"/>; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Interval<T> a, Interval<T> b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Returns whether an interval is less than another.
        /// </summary>
        /// <param name="a">The first interval.</param>
        /// <param name="b">The second interval.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="a"/> is less than <paramref name="b"/>; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public static bool operator <(Interval<T> a, Interval<T> b)
        {
            return Comparer<Interval<T>>.Default.Compare(a, b) < 0;
        }

        /// <summary>
        /// Returns whether an interval is less than or equal to another.
        /// </summary>
        /// <param name="a">The first interval.</param>
        /// <param name="b">The second interval.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="a"/> is less than or equal to <paramref name="b"/>; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public static bool operator <=(Interval<T> a, Interval<T> b)
        {
            return Comparer<Interval<T>>.Default.Compare(a, b) <= 0;
        }

        /// <summary>
        /// Returns whether two intervals are equal.
        /// </summary>
        /// <param name="a">The first interval.</param>
        /// <param name="b">The second interval.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="a"/> is equal to <paramref name="b"/>; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Interval<T> a, Interval<T> b)
        {
            return EqualityComparer<Interval<T>>.Default.Equals(a, b);
        }

        /// <summary>
        /// Returns whether an interval is greater than another.
        /// </summary>
        /// <param name="a">The first interval.</param>
        /// <param name="b">The second interval.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="a"/> is greater than <paramref name="b"/>; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public static bool operator >(Interval<T> a, Interval<T> b)
        {
            return Comparer<Interval<T>>.Default.Compare(a, b) > 0;
        }

        /// <summary>
        /// Returns whether an interval is greater than or equal to another.
        /// </summary>
        /// <param name="a">The first interval.</param>
        /// <param name="b">The second interval.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="a"/> is greater than or equal to <paramref name="b"/>; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public static bool operator >=(Interval<T> a, Interval<T> b)
        {
            return Comparer<Interval<T>>.Default.Compare(a, b) >= 0;
        }

        /// <summary>
        /// Initializes a new right-bounded instance of the <see cref="Interval{T}"/> class.
        /// </summary>
        /// <param name="right">The right endpoint.</param>
        /// <param name="isRightClosed">A value indicating whether the right endpoint is closed.</param>
        /// <returns>An <see cref="Interval{T}"/> instance.</returns>
        public static Interval<T> RightBounded(T right, bool isRightClosed)
        {
            return new Interval<T>(
                null, EndpointType.Unbounded, right, isRightClosed ? EndpointType.Closed : EndpointType.Open);
        }

        /// <summary>
        /// Initializes a new unbounded instance of the <see cref="Interval{T}"/> class.
        /// </summary>
        /// <returns>An <see cref="Interval{T}"/> instance.</returns>
        public static Interval<T> Unbounded()
        {
            return new Interval<T>(null, EndpointType.Unbounded, null, EndpointType.Unbounded);
        }

        /// <summary>
        /// Determines whether this interval is adjacent to another.
        /// </summary>
        /// <param name="interval">The other interval.</param>
        /// <returns>
        /// <see langword="true"/> if this interval is adjacent to the other; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Abuts(Interval<T> interval)
        {
            return (interval != null) && !this.Overlaps(interval) && (this.Gap(interval) == null);
        }

        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// Less than zero if this instance is less than <paramref name="obj"/>, zero if they are equal, and greater
        /// than zero if this instance is greater than <paramref name="obj"/>.
        /// </returns>
        public int CompareTo(object obj)
        {
            return this.CompareTo(obj as Interval<T>);
        }

        /// <summary>
        /// Compares this interval with another.
        /// </summary>
        /// <param name="other">An interval to compare with this one.</param>
        /// <returns>
        /// Less than zero if this interval is less than <paramref name="other"/>, zero if they are equal, and greater
        /// than zero if this interval is greater than <paramref name="other"/>.
        /// </returns>
        public int CompareTo(Interval<T> other)
        {
            if (other == null)
            {
                return 1;
            }

            if (this.Left.Equals(other.Left) && this.LeftType.Equals(other.LeftType))
            {
                if (this.Right.Equals(other.Right) && this.RightType.Equals(other.RightType))
                {
                    return 0;
                }

                return
                    ((this.RightType != EndpointType.Unbounded) &&
                    other.RightIncludes(this.Right.Value, this.RightType == EndpointType.Open)) ?
                        1 : -1;
            }

            return
                ((this.LeftType != EndpointType.Unbounded) &&
                other.LeftIncludes(this.Left.Value, this.LeftType == EndpointType.Open)) ?
                    1 : -1;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare with this instance.</param>
        /// <returns>
        /// <see langword="true"/> if the specified <see cref="Object"/> is equal to this instance; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Interval<T>);
        }

        /// <summary>
        /// Indicates whether this interval is equal to another.
        /// </summary>
        /// <param name="other">An interval to compare with this one.</param>
        /// <returns>
        /// <see langword="true"/> if this interval is equal to <paramref name="other"/>; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public bool Equals(Interval<T> other)
        {
            return (other == null) ? false : this.GetHashCode() == other.GetHashCode();
        }

        /// <summary>
        /// Determines whether a gap exists between this interval and another.
        /// </summary>
        /// <param name="other">The other interval.</param>
        /// <returns>
        /// An <see cref="Interval{T}"/> instance if there is a gap; otherwise, <see langword="null"/>.
        /// </returns>
        public Interval<T> Gap(Interval<T> other)
        {
            if (!this.Overlaps(other) && (other != null))
            {
                Interval<T> lower, higher;

                if (this.CompareTo(other) < 0)
                {
                    lower = this;
                    higher = other;
                }
                else
                {
                    lower = other;
                    higher = this;
                }

                var left = lower.Right.Value;
                var isLeftClosed = lower.RightType == EndpointType.Open;
                var right = higher.Left.Value;
                var isRightClosed = higher.LeftType == EndpointType.Open;

                return IsEmpty(left, isLeftClosed, right, isRightClosed) ?
                    null :
                    Interval<T>.Bounded(left, isLeftClosed, right, isRightClosed);
            }

            return null;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash
        /// table. 
        /// </returns>
        public override int GetHashCode()
        {
            return
                (this.Left.HasValue ? this.Left.GetHashCode() : 0) ^
                (this.Right.HasValue ? this.Right.GetHashCode() : 0) ^
                this.endpoints.GetHashCode();
        }

        /// <summary>
        /// Determines whether the interval includes a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// <see langword="true"/> if the interval includes the value; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Includes(T value)
        {
            return this.LeftIncludes(value, true) && this.RightIncludes(value, true);
        }

        /// <summary>
        /// Determines whether the interval includes another.
        /// </summary>
        /// <param name="other">An interval to check for inclusion in the current one.</param>
        /// <returns>
        /// <see langword="true"/> if the interval contains the other; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Includes(Interval<T> other)
        {
            if (other == null)
            {
                return false;
            }

            if ((this.LeftType != EndpointType.Unbounded) &&
                other.LeftIncludes(this.Left.Value, this.LeftType == EndpointType.Open))
            {
                return false;
            }

            if ((this.RightType != EndpointType.Unbounded) &&
                other.RightIncludes(this.Right.Value, this.RightType == EndpointType.Open))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether the interval is partitioned by a set of intervals.
        /// </summary>
        /// <param name="intervals">The intervals.</param>
        /// <returns>
        /// <see langword="true"/> if the interval is partitioned by the set; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="intervals"/> is <see langword="null"/>.</exception>
        public bool IsPartitionedBy(IEnumerable<Interval<T>> intervals)
        {
            intervals.AssertNotNull("intervals");

            return !IsContiguous(intervals) ? false : this.Equals(Combine(intervals));
        }

        /// <summary>
        /// Determines whether this interval overlaps another.
        /// </summary>
        /// <param name="other">The other interval.</param>
        /// <returns>
        /// <see langword="true"/> if this interval overlaps the other; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Overlaps(Interval<T> other)
        {
            if (other == null)
            {
                return false;
            }

            var otherIncludesLeft =
                (this.LeftType != EndpointType.Unbounded) && other.LeftIncludes(this.Left.Value, true) &&
                    other.RightIncludes(this.Left.Value, this.LeftType == EndpointType.Closed);

            var otherIncludesRight =
                (this.RightType != EndpointType.Unbounded) && other.RightIncludes(this.Right.Value, true) &&
                    other.LeftIncludes(this.Right.Value, this.RightType == EndpointType.Closed);

            return otherIncludesLeft || otherIncludesRight || this.Includes(other);
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents the current <see cref="Interval{T}"/>.
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current <see cref="Interval{T}"/>.</returns>
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            if (this.LeftType == EndpointType.Unbounded)
            {
                stringBuilder.Append("(-∞");
            }
            else
            {
                stringBuilder.Append((this.LeftType == EndpointType.Open) ? '(' : '[');
                stringBuilder.Append(this.Left.Value);
            }

            stringBuilder.Append(',');

            if (this.RightType == EndpointType.Unbounded)
            {
                stringBuilder.Append("∞)");
            }
            else
            {
                stringBuilder.Append(this.Right.Value);
                stringBuilder.Append((this.RightType == EndpointType.Open) ? ')' : ']');
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Closes a discrete interval endpoint.
        /// </summary>
        /// <remarks>
        /// Discrete intervals are rarely displayed using open endpoints. This method updates an open endpoint with
        /// its equivalent closed representation (e.g. ]0,5] => [1,5] or [0,5[ => [0,4]).
        /// </remarks>
        /// <param name="value">The endpoint value.</param>
        /// <param name="type">The endpoint type.</param>
        /// <param name="adjustment">The adjustment to be made on the endpoint (i.e. 1 or -1).</param>
        private static void CloseEndpoint(ref T? value, ref EndpointType type, short adjustment)
        {
            if (type == EndpointType.Open)
            {
                if (value is short?)
                {
                    // Using the simpler conversion syntax causes unboxing errors with shorts.
                    value = (T?)Convert.ChangeType((short)((short)(object)value.Value + adjustment), typeof(object));
                    type = EndpointType.Closed;
                }
                else if (value is int?)
                {
                    value = (T?)(object)((int)(object)value.Value + adjustment);
                    type = EndpointType.Closed;
                }
                else if (value is long?)
                {
                    value = (T?)(object)((long)(object)value.Value + adjustment);
                    type = EndpointType.Closed;
                }
            }
        }

        /// <summary>
        /// Determines whether the interval's left endpoint includes a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="included">A value indicating whether <paramref name="value"/> is included.</param>
        /// <returns>
        /// <see langword="true"/> if the interval's left endpoint includes the value; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        private bool LeftIncludes(T value, bool included)
        {
            return
                (this.LeftType == EndpointType.Unbounded) ||
                ((this.LeftType == EndpointType.Closed) && this.Left.Value.Equals(value) && included) ||
                (this.Left.Value.CompareTo(value) < 0);
        }

        /// <summary>
        /// Determines whether the interval's right endpoint includes a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="included">A value indicating whether <paramref name="value"/> is included.</param>
        /// <returns>
        /// <see langword="true"/> if the interval's right endpoint includes the value; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        private bool RightIncludes(T value, bool included)
        {
            return
                (this.RightType == EndpointType.Unbounded) ||
                ((this.RightType == EndpointType.Closed) && this.Right.Value.Equals(value) && included) ||
                (this.Right.Value.CompareTo(value) > 0);
        }
    }
}