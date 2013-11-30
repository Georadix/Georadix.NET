namespace Georadix.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using Xunit;
    using Xunit.Extensions;

    public class DoubleRangeFixture
    {
        [Theory]
        [InlineData(1.11, 1.11, 1.11, 1.11, false)]
        [InlineData(-5.02, 10.123, 10.122, 12, false)]
        [InlineData(-5.02, 10.123, 10.123, 12, true)]
        [InlineData(-5.02, 10.123, -7, -5.02, true)]
        [InlineData(50, -10, 1, 1, false)]
        public void AbutsReturnsExpectedResult(
            double start, double end, double otherStart, double otherEnd, bool expected)
        {
            var sut = new DoubleRange(start, end);
            var otherRange = new DoubleRange(otherStart, otherEnd);

            Assert.Equal(expected, sut.Abuts(otherRange));
        }

        [Fact]
        public void AbutsWithNullRangeThrowsArgumentNullException()
        {
            var sut = new DoubleRange();
            DoubleRange nullRange = null;

            Assert.Throws<ArgumentNullException>(() => sut.Abuts(nullRange));
        }

        [Fact]
        public void ContainsNullRangeThrowsArgumentNullException()
        {
            var sut = new DoubleRange();
            DoubleRange nullRange = null;

            Assert.Throws<ArgumentNullException>(() => sut.Contains(nullRange));
        }

        [Theory]
        [InlineData(1.01, 1.01, 1.01, 1.01, false)]
        [InlineData(-5.02, 10.123, 9.5, 12, false)]
        [InlineData(-5.02, 10.123, 10.122, 12, false)]
        [InlineData(-5.02, 10.123, 10.122, 10.123, true)]
        [InlineData(-5.02, 10.123, -5.02, -4, true)]
        [InlineData(50, -10, 1, 1, false)]
        public void ContainsOtherRangeReturnsExpectedResult(
            double start, double end, double otherStart, double otherEnd, bool expected)
        {
            var sut = new DoubleRange(start, end);
            var otherRange = new DoubleRange(otherStart, otherEnd);

            Assert.Equal(expected, sut.Contains(otherRange));
        }

        [Fact]
        public void CreateDefaultIsEmpty()
        {
            var sut = new DoubleRange();

            Assert.True(sut.IsEmpty);
        }

        [Theory]
        [InlineData(-5.1, 5.5)]
        [InlineData(0, 10.15)]
        [InlineData(500.00001, -150)]
        public void CreateFromOtherRangeIsProperlyInitialized(double start, double end)
        {
            var otherRange = new DoubleRange(start, end);
            var sut = new DoubleRange(otherRange);

            Assert.Equal(start, sut.Start);
            Assert.Equal(end, sut.End);
        }

        [Theory]
        [InlineData(-5.1, 5.5)]
        [InlineData(0, 10.15)]
        [InlineData(500.00001, -150)]
        public void CreateFromValuesIsProperlyInitialized(double start, double end)
        {
            var sut = new DoubleRange(start, end);

            Assert.Equal(start, sut.Start);
            Assert.Equal(end, sut.End);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1500.12345678)]
        public void EndingAtIsProperlyInitialized(double end)
        {
            var sut = DoubleRange.EndingAt(end);

            Assert.Equal(double.MinValue, sut.Start);
            Assert.Equal(end, sut.End);
        }

        [Theory]
        [InlineData(1.1, 1.1, 1.1, 1.1)]
        [InlineData(-5.02, 10.123, 10.123, 12)]
        [InlineData(-5, 10, 50, -10)]
        [InlineData(50, -10, 1, 1)]
        public void GapReturnsEmptyRange(double start, double end, double otherStart, double otherEnd)
        {
            var sut = new DoubleRange(start, end);
            var otherRange = new DoubleRange(otherStart, otherEnd);

            Assert.True(sut.Gap(otherRange).IsEmpty);
        }

        [Theory]
        [InlineData(-5.02, 10.123, 12, 13, 10.123, 12)]
        [InlineData(-5.02, 10.123, -15, -14, -14, -5.02)]
        public void GapReturnsExpectedResult(
            double start,
            double end,
            double otherStart,
            double otherEnd,
            double expectedGapStart,
            double expectedGapEnd)
        {
            var sut = new DoubleRange(start, end);
            var otherRange = new DoubleRange(otherStart, otherEnd);
            var expected = new DoubleRange(expectedGapStart, expectedGapEnd);

            Assert.Equal(expected, sut.Gap(otherRange));
        }

        [Fact]
        public void GapWithNullRangeThrowsArgumentNullException()
        {
            var sut = new DoubleRange();
            DoubleRange nullRange = null;

            Assert.Throws<ArgumentNullException>(() => sut.Gap(nullRange));
        }

        [Theory]
        [InlineData(1.1, 1.1, true)]
        [InlineData(100, -100, true)]
        [InlineData(-100.0001, 100.0001, false)]
        public void IsEmptyReturnsExpectedResult(double start, double end, bool expected)
        {
            var sut = new DoubleRange(start, end);

            Assert.Equal(expected, sut.IsEmpty);
        }

        [Fact]
        public void IsPartitionedByEqualContiguousRangesReturnsTrue()
        {
            var sut = new DoubleRange(-5.02, 5.02);
            var ranges = new List<DoubleRange>();
            ranges.Add(new DoubleRange(-5.02, 0));
            ranges.Add(new DoubleRange(0, 1));
            ranges.Add(new DoubleRange(1, 5.02));

            Assert.True(sut.IsPartitionedBy(ranges));
        }

        [Fact]
        public void IsPartitionedByNonContiguousRangesReturnsFalse()
        {
            var sut = new DoubleRange(-5.02, 5.02);
            var ranges = new List<DoubleRange>();
            ranges.Add(new DoubleRange(-5.02, 0));
            ranges.Add(new DoubleRange(0.000001, 1));
            ranges.Add(new DoubleRange(1, 5.02));

            Assert.False(sut.IsPartitionedBy(ranges));
        }

        [Fact]
        public void IsPartitionedByNullRangesThrowsArgumentNullException()
        {
            var sut = new DoubleRange();
            List<DoubleRange> nullRanges = null;

            Assert.Throws<ArgumentNullException>(() => sut.IsPartitionedBy(nullRanges));
        }

        [Fact]
        public void IsPartitionedByNullValuesInRangesThrowsArgumentException()
        {
            var sut = new DoubleRange();
            var ranges = new List<DoubleRange>();
            ranges.Add(null);

            Assert.Throws<ArgumentException>(() => sut.IsPartitionedBy(ranges));
        }

        [Theory]
        [InlineData(1.1, 1.1, 0)]
        [InlineData(10, -10, 0)]
        [InlineData(-10.001, 10.1, 20.101)]
        public void LengthReturnsExpectedResult(double start, double end, double expected)
        {
            var sut = new DoubleRange(start, end);

            Assert.Equal(expected, sut.Length);
        }

        [Fact]
        public void OverlapsNullRangeThrowsArgumentNullException()
        {
            var sut = new DoubleRange();
            DoubleRange nullRange = null;

            Assert.Throws<ArgumentNullException>(() => sut.Overlaps(nullRange));
        }

        [Theory]
        [InlineData(1.1, 1.1, 1.1, 1.1, false)]
        [InlineData(-5.02, 10.123, 10.123, 12, false)]
        [InlineData(-5.02, 10.123, 10.122, 12, true)]
        [InlineData(-5.02, 10.123, -7, -5.0199999, true)]
        [InlineData(50, -10, 1, 1, false)]
        public void OverlapsReturnsExpectedResult(
            double start, double end, double otherStart, double otherEnd, bool expected)
        {
            var sut = new DoubleRange(start, end);
            var otherRange = new DoubleRange(otherStart, otherEnd);

            Assert.Equal(expected, sut.Overlaps(otherRange));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(50.002)]
        public void StartingAtIsProperlyInitialized(double start)
        {
            var sut = DoubleRange.StartingAt(start);

            Assert.Equal(start, sut.Start);
            Assert.Equal(double.MaxValue, sut.End);
        }
    }
}