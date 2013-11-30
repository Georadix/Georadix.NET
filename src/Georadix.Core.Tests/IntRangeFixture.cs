namespace Georadix.Core.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using Xunit.Extensions;

    public class IntRangeFixture
    {
        [Theory]
        [InlineData(1, 1, 1, 1, false)]
        [InlineData(-5, 10, 10, 12, false)]
        [InlineData(-5, 10, 11, 12, true)]
        [InlineData(-5, 10, -7, -6, true)]
        [InlineData(50, -10, 1, 1, false)]
        public void AbutsReturnsExpectedResult(int start, int end, int otherStart, int otherEnd, bool expected)
        {
            var sut = new IntRange(start, end);
            var otherRange = new IntRange(otherStart, otherEnd);

            Assert.Equal(expected, sut.Abuts(otherRange));
        }

        [Fact]
        public void AbutsWithNullRangeThrowsArgumentNullException()
        {
            var sut = new IntRange();
            IntRange nullRange = null;

            Assert.Throws<ArgumentNullException>(() => sut.Abuts(nullRange));
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(-5, 10)]
        [InlineData(50, -10)]
        public void ContainsEqualsLinqContains(int start, int end)
        {
            var sut = new IntRange(start, end);
            var values = new List<int>();

            values.Add(sut.Start - 1);
            values.Add(sut.Start);
            values.Add(sut.End);
            values.Add((sut.End - sut.Start) / 2);
            values.Add(sut.End + 1);

            foreach (var value in values)
            {
                Assert.Equal(((IEnumerable<int>)sut).Contains(value), sut.Contains(value));
            }
        }

        [Fact]
        public void ContainsNullRangeThrowsArgumentNullException()
        {
            var sut = new IntRange();
            IntRange nullRange = null;

            Assert.Throws<ArgumentNullException>(() => sut.Contains(nullRange));
        }

        [Theory]
        [InlineData(1, 1, 1, 1, true)]
        [InlineData(-5, 10, 9, 12, false)]
        [InlineData(50, -10, 1, 1, false)]
        public void ContainsOtherRangeReturnsExpectedResult(
            int start, int end, int otherStart, int otherEnd, bool expected)
        {
            var sut = new IntRange(start, end);
            var otherRange = new IntRange(otherStart, otherEnd);

            Assert.Equal(expected, sut.Contains(otherRange));
        }

        [Fact]
        public void CreateDefaultIsEmpty()
        {
            var sut = new IntRange();

            Assert.True(sut.IsEmpty);
        }

        [Theory]
        [InlineData(-5, 5)]
        [InlineData(0, 10)]
        [InlineData(500, -150)]
        public void CreateFromOtherRangeIsProperlyInitialized(int start, int end)
        {
            var otherRange = new IntRange(start, end);
            var sut = new IntRange(otherRange);

            Assert.Equal(start, sut.Start);
            Assert.Equal(end, sut.End);
        }

        [Theory]
        [InlineData(-5, 5)]
        [InlineData(0, 10)]
        [InlineData(500, -150)]
        public void CreateFromValuesIsProperlyInitialized(int start, int end)
        {
            var sut = new IntRange(start, end);

            Assert.Equal(start, sut.Start);
            Assert.Equal(end, sut.End);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1500)]
        public void EndingAtIsProperlyInitialized(int end)
        {
            var sut = IntRange.EndingAt(end);

            Assert.Equal(int.MinValue, sut.Start);
            Assert.Equal(end, sut.End);
        }

        [Theory]
        [InlineData(int.MaxValue, int.MinValue, new int[] { })]
        [InlineData(1, 1, new int[] { 1 })]
        [InlineData(0, 5, new int[] { 0, 1, 2, 3, 4, 5 })]
        public void EnumeratorYieldsExpectedValues(int start, int end, int[] expected)
        {
            var sut = new IntRange(start, end);

            // Test the IEnumerable<int> interface.
            Assert.True(sut.SequenceEqual(expected));

            // Test the IEnumerable interface.
            var items = new List<int>();
            foreach (int value in (IEnumerable)sut)
            {
                items.Add(value);
            }

            Assert.True(items.SequenceEqual(sut));
        }

        [Theory]
        [InlineData(1, 1, 1, 1)]
        [InlineData(-5, 10, 11, 12)]
        [InlineData(-5, 10, 50, -10)]
        [InlineData(50, -10, 1, 1)]
        public void GapReturnsEmptyRange(int start, int end, int otherStart, int otherEnd)
        {
            var sut = new IntRange(start, end);
            var otherRange = new IntRange(otherStart, otherEnd);

            Assert.True(sut.Gap(otherRange).IsEmpty);
        }

        [Theory]
        [InlineData(1, 1, 1, 1, int.MaxValue, int.MinValue)]
        [InlineData(-5, 10, 11, 12, 11, 10)]
        [InlineData(-5, 10, 12, 13, 11, 11)]
        [InlineData(-5, 10, -15, -14, -13, -6)]
        [InlineData(-5, 10, 50, -10, int.MaxValue, int.MinValue)]
        [InlineData(50, -10, 1, 1, int.MaxValue, int.MinValue)]
        public void GapReturnsExpectedResult(
            int start, int end, int otherStart, int otherEnd, int expectedGapStart, int expectedGapEnd)
        {
            var sut = new IntRange(start, end);
            var otherRange = new IntRange(otherStart, otherEnd);
            var expected = new IntRange(expectedGapStart, expectedGapEnd);

            Assert.Equal(expected, sut.Gap(otherRange));
        }

        [Fact]
        public void GapWithNullRangeThrowsArgumentNullException()
        {
            var sut = new IntRange();
            IntRange nullRange = null;

            Assert.Throws<ArgumentNullException>(() => sut.Gap(nullRange));
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(100, -100)]
        [InlineData(-100, 100)]
        public void IsEmptyEqualsLinqCountOfZero(int start, int end)
        {
            var sut = new IntRange(start, end);

            Assert.Equal(sut.Count() == 0, sut.IsEmpty);
        }

        [Fact]
        public void IsPartitionedByEqualContiguousRangesReturnsTrue()
        {
            var sut = new IntRange(-5, 5);
            var ranges = new List<IntRange>();
            ranges.Add(new IntRange(-5, 0));
            ranges.Add(new IntRange(1, 1));
            ranges.Add(new IntRange(2, 5));

            Assert.True(sut.IsPartitionedBy(ranges));
        }

        [Fact]
        public void IsPartitionedByNonContiguousRangesReturnsFalse()
        {
            var sut = new IntRange(-5, 5);
            var ranges = new List<IntRange>();
            ranges.Add(new IntRange(-5, 0));
            ranges.Add(new IntRange(0, 1));
            ranges.Add(new IntRange(2, 5));

            Assert.False(sut.IsPartitionedBy(ranges));
        }

        [Fact]
        public void IsPartitionedByNullRangesThrowsArgumentNullException()
        {
            var sut = new IntRange();
            List<IntRange> nullRanges = null;

            Assert.Throws<ArgumentNullException>(() => sut.IsPartitionedBy(nullRanges));
        }

        [Fact]
        public void IsPartitionedByNullValuesInRangesThrowsArgumentException()
        {
            var sut = new IntRange();
            var ranges = new List<IntRange>();
            ranges.Add(null);

            Assert.Throws<ArgumentException>(() => sut.IsPartitionedBy(ranges));
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(10, -10)]
        [InlineData(-10, 10)]
        public void LengthEqualsLinqCount(int start, int end)
        {
            var sut = new IntRange(start, end);

            Assert.Equal(sut.Count(), sut.Length);
        }

        [Fact]
        public void OverlapsNullRangeThrowsArgumentNullException()
        {
            var sut = new IntRange();
            IntRange nullRange = null;

            Assert.Throws<ArgumentNullException>(() => sut.Overlaps(nullRange));
        }

        [Theory]
        [InlineData(1, 1, 1, 1, true)]
        [InlineData(-5, 10, 10, 12, true)]
        [InlineData(-5, 10, -7, -5, true)]
        [InlineData(50, -10, 1, 1, false)]
        public void OverlapsReturnsExpectedResult(int start, int end, int otherStart, int otherEnd, bool expected)
        {
            var sut = new IntRange(start, end);
            var otherRange = new IntRange(otherStart, otherEnd);

            Assert.Equal(expected, sut.Overlaps(otherRange));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(50)]
        public void StartingAtIsProperlyInitialized(int start)
        {
            var sut = IntRange.StartingAt(start);

            Assert.Equal(start, sut.Start);
            Assert.Equal(int.MaxValue, sut.End);
        }
    }
}