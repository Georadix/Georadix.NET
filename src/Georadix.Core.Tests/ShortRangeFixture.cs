namespace Georadix.Core.Tests
{
    using System.Linq;
    using Xunit;
    using Xunit.Extensions;

    public class ShortRangeFixture
    {
        [Fact]
        public void CreateDefaultIsEmpty()
        {
            var sut = new ShortRange();

            Assert.True(sut.IsEmpty);
        }

        [Theory]
        [InlineData((short)-5, (short)5)]
        [InlineData((short)0, (short)10)]
        [InlineData((short)500, (short)-150)]
        public void CreateFromOtherRangeIsProperlyInitialized(short start, short end)
        {
            var otherRange = new ShortRange(start, end);
            var sut = new ShortRange(otherRange);

            Assert.Equal(start, sut.Start);
            Assert.Equal(end, sut.End);
        }

        [Theory]
        [InlineData((short)-5, (short)5)]
        [InlineData((short)0, (short)10)]
        [InlineData((short)500, (short)-150)]
        public void CreateFromValuesIsProperlyInitialized(short start, short end)
        {
            var sut = new ShortRange(start, end);

            Assert.Equal(start, sut.Start);
            Assert.Equal(end, sut.End);
        }

        [Theory]
        [InlineData((short)0)]
        [InlineData((short)-1500)]
        public void EndingAtIsProperlyInitialized(short end)
        {
            var sut = ShortRange.EndingAt(end);

            Assert.Equal(short.MinValue, sut.Start);
            Assert.Equal(end, sut.End);
        }

        [Theory]
        [InlineData((short)1, (short)1, (short)1, (short)1)]
        [InlineData((short)-5, (short)10, (short)11, (short)12)]
        [InlineData((short)-5, (short)10, (short)50, (short)-10)]
        [InlineData((short)50, (short)-10, (short)1, (short)1)]
        public void GapReturnsEmptyRange(short start, short end, short otherStart, short otherEnd)
        {
            var sut = new ShortRange(start, end);
            var otherRange = new ShortRange(otherStart, otherEnd);

            Assert.True(sut.Gap(otherRange).IsEmpty);
        }

        [Theory]
        [InlineData((short)1, (short)1, (short)1, (short)1, short.MaxValue, short.MinValue)]
        [InlineData((short)-5, (short)10, (short)11, (short)12, (short)11, (short)10)]
        [InlineData((short)-5, (short)10, (short)12, (short)13, (short)11, (short)11)]
        [InlineData((short)-5, (short)10, (short)-15, (short)-14, (short)-13, (short)-6)]
        [InlineData((short)-5, (short)10, (short)50, (short)-10, short.MaxValue, short.MinValue)]
        [InlineData((short)50, (short)-10, (short)1, (short)1, short.MaxValue, short.MinValue)]
        public void GapReturnsExpectedResult(
            short start, short end, short otherStart, short otherEnd, short expectedGapStart, short expectedGapEnd)
        {
            var sut = new ShortRange(start, end);
            var otherRange = new ShortRange(otherStart, otherEnd);
            var expected = new ShortRange(expectedGapStart, expectedGapEnd);

            Assert.Equal(expected, sut.Gap(otherRange));
        }

        [Theory]
        [InlineData((short)1, (short)1)]
        [InlineData((short)10, (short)-10)]
        [InlineData((short)-10, (short)10)]
        public void LengthEqualsLinqCount(short start, short end)
        {
            var sut = new ShortRange(start, end);

            Assert.Equal(sut.Count(), sut.Length);
        }

        [Theory]
        [InlineData((short)0)]
        [InlineData((short)50)]
        public void StartingAtIsProperlyInitialized(short start)
        {
            var sut = ShortRange.StartingAt(start);

            Assert.Equal(start, sut.Start);
            Assert.Equal(short.MaxValue, sut.End);
        }
    }
}