namespace Georadix.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using Xunit;
    using Xunit.Extensions;

    public class DateTimeRangeFixture
    {
        public static IEnumerable<object[]> ConstructorScenarios
        {
            get
            {
                var now = DateTime.Now;

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
                        new DateTime(0)
                    },
                    new object[]
                    {
                        DateTime.Now
                    }
                };
            }
        }

        public static IEnumerable<object[]> LengthScenarios
        {
            get
            {
                var now = DateTime.Now;

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
            var sut = new DateTimeRange();

            Assert.True(sut.IsEmpty);
        }

        [Theory]
        [PropertyData("ConstructorScenarios")]
        public void CreateFromOtherRangeIsProperlyInitialized(DateTime start, DateTime end)
        {
            var otherRange = new DateTimeRange(start, end);
            var sut = new DateTimeRange(otherRange);

            Assert.Equal(start, sut.Start);
            Assert.Equal(end, sut.End);
        }

        [Theory]
        [PropertyData("ConstructorScenarios")]
        public void CreateFromValuesIsProperlyInitialized(DateTime start, DateTime end)
        {
            var sut = new DateTimeRange(start, end);

            Assert.Equal(start, sut.Start);
            Assert.Equal(end, sut.End);
        }

        [Fact]
        public void EmptyRangeHasLengthOfZero()
        {
            var sut = new DateTimeRange();

            Assert.Equal(TimeSpan.Zero, sut.Length);
        }

        [Theory]
        [PropertyData("CreateAtScenarios")]
        public void EndingAtIsProperlyInitialized(DateTime end)
        {
            var sut = DateTimeRange.EndingAt(end);

            Assert.Equal(DateTime.MinValue, sut.Start);
            Assert.Equal(end, sut.End);
        }

        [Theory]
        [PropertyData("LengthScenarios")]
        public void NonEmptyRangeHasExpectedLength(DateTime start, DateTime end, TimeSpan expected)
        {
            var sut = new DateTimeRange(start, end);

            Assert.Equal(expected, sut.Length);
        }

        [Theory]
        [PropertyData("CreateAtScenarios")]
        public void StartingAtIsProperlyInitialized(DateTime start)
        {
            var sut = DateTimeRange.StartingAt(start);

            Assert.Equal(start, sut.Start);
            Assert.Equal(DateTime.MaxValue, sut.End);
        }
    }
}
