namespace Georadix.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using Xunit;
    using Xunit.Extensions;

    public class DateTimeOffsetRangeFixture
    {
        public static IEnumerable<object[]> ConstructorScenarios
        {
            get
            {
                var now = DateTimeOffset.Now;

                return new object[][]
                {
                    new object[]
                    {
                        now,
                        now.AddYears(1)
                    },
                    new object[]
                    {
                        now.AddHours(-1),
                        now
                    },
                    new object[]
                    {
                        now,
                        now.AddMinutes(-30)
                    }
                };
            }
        }

        public static IEnumerable<object[]> CreateAtScenarios
        {
            get
            {
                return new object[][]
                {
                    new object[]
                    {
                        new DateTimeOffset(0, TimeSpan.Zero)
                    },
                    new object[]
                    {
                        DateTimeOffset.Now
                    }
                };
            }
        }

        public static IEnumerable<object[]> LengthScenarios
        {
            get
            {
                var now = DateTimeOffset.Now;

                return new object[][]
                {
                    new object[]
                    {
                        now,
                        now.AddDays(3),
                        TimeSpan.FromDays(3)
                    },
                    new object[]
                    {
                        now.AddHours(-6.5),
                        now,
                        TimeSpan.FromHours(6.5)
                    }
                };
            }
        }

        [Fact]
        public void CreateDefaultIsEmpty()
        {
            var sut = new DateTimeOffsetRange();

            Assert.True(sut.IsEmpty);
        }

        [Theory]
        [PropertyData("ConstructorScenarios")]
        public void CreateFromOtherRangeIsProperlyInitialized(DateTimeOffset start, DateTimeOffset end)
        {
            var otherRange = new DateTimeOffsetRange(start, end);
            var sut = new DateTimeOffsetRange(otherRange);

            Assert.Equal(start, sut.Start);
            Assert.Equal(end, sut.End);
        }

        [Theory]
        [PropertyData("ConstructorScenarios")]
        public void CreateFromValuesIsProperlyInitialized(DateTimeOffset start, DateTimeOffset end)
        {
            var sut = new DateTimeOffsetRange(start, end);

            Assert.Equal(start, sut.Start);
            Assert.Equal(end, sut.End);
        }

        [Fact]
        public void EmptyRangeHasLengthOfZero()
        {
            var sut = new DateTimeOffsetRange();

            Assert.Equal(TimeSpan.Zero, sut.Length);
        }

        [Theory]
        [PropertyData("CreateAtScenarios")]
        public void EndingAtIsProperlyInitialized(DateTimeOffset end)
        {
            var sut = DateTimeOffsetRange.EndingAt(end);

            Assert.Equal(DateTimeOffset.MinValue, sut.Start);
            Assert.Equal(end, sut.End);
        }

        [Theory]
        [PropertyData("LengthScenarios")]
        public void NonEmptyRangeHasExpectedLength(DateTimeOffset start, DateTimeOffset end, TimeSpan expected)
        {
            var sut = new DateTimeOffsetRange(start, end);

            Assert.Equal(expected, sut.Length);
        }

        [Theory]
        [PropertyData("CreateAtScenarios")]
        public void StartingAtIsProperlyInitialized(DateTimeOffset start)
        {
            var sut = DateTimeOffsetRange.StartingAt(start);

            Assert.Equal(start, sut.Start);
            Assert.Equal(DateTimeOffset.MaxValue, sut.End);
        }
    }
}