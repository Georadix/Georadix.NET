namespace Georadix.Core
{
    using System;
    using System.Collections.Generic;
    using Xunit;
    using Xunit.Extensions;

    public class IntervalFixture
    {
        public static IEnumerable<object[]> AbutsScenarios
        {
            get
            {
                return new object[][]
                {
                    new object[]
                    {
                        Interval<double>.Unbounded(),
                        Interval<double>.Unbounded(),
                        false
                    },
                    new object[]
                    {
                        Interval<double>.Unbounded(),
                        null,
                        false
                    },
                    new object[]
                    {
                        Interval<double>.LeftBounded(0, true),
                        Interval<double>.RightBounded(0, true),
                        false
                    },
                    new object[]
                    {
                        Interval<double>.LeftBounded(0, false),
                        Interval<double>.RightBounded(0, true),
                        true
                    },
                    new object[]
                    {
                        Interval<double>.RightBounded(0, false),
                        Interval<double>.LeftBounded(0, true),
                        true
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, true, 1, true),
                        Interval<double>.Bounded(-1, true, 1, true),
                        false
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, false, 1, true),
                        Interval<double>.Bounded(-2, true, -1, true),
                        true
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, true, 1, true),
                        Interval<double>.Bounded(1, false, 2, true),
                        true
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, true, 1, true),
                        Interval<double>.Bounded(-3, true, -2, true),
                        false
                    }
                };
            }
        }

        public static IEnumerable<object[]> BoundedEmptyIntervalScenarios
        {
            get
            {
                return new object[][]
                {
                    new object[] { 1, true, -1, true },
                    new object[] { 0, true, 0, false },
                    new object[] { 0, false, 0, true },
                    new object[] { 0, false, 0, false }
                };
            }
        }

        public static IEnumerable<object[]> BoundedIntScenarios
        {
            get
            {
                return new object[][]
                {
                    new object[] { -1, true, 1, true, Interval<int>.Bounded(-1, true, 1, true) },
                    new object[] { -1, true, 1, false, Interval<int>.Bounded(-1, true, 0, true) },
                    new object[] { -1, false, 1, true, Interval<int>.Bounded(0, true, 1, true) },
                    new object[] { -1, false, 1, false, Interval<int>.Bounded(0, true, 0, true) }
                };
            }
        }

        public static IEnumerable<object[]> BoundedLongScenarios
        {
            get
            {
                return new object[][]
                {
                    new object[] { -1, true, 1, true, Interval<long>.Bounded(-1, true, 1, true) },
                    new object[] { -1, true, 1, false, Interval<long>.Bounded(-1, true, 0, true) },
                    new object[] { -1, false, 1, true, Interval<long>.Bounded(0, true, 1, true) },
                    new object[] { -1, false, 1, false, Interval<long>.Bounded(0, true, 0, true) }
                };
            }
        }

        public static IEnumerable<object[]> BoundedShortScenarios
        {
            get
            {
                return new object[][]
                {
                    new object[] { (short)-1, true, (short)1, true, Interval<short>.Bounded(-1, true, 1, true) },
                    new object[] { (short)-1, true, (short)1, false, Interval<short>.Bounded(-1, true, 0, true) },
                    new object[] { (short)-1, false, (short)1, true, Interval<short>.Bounded(0, true, 1, true) },
                    new object[] { (short)-1, false, (short)1, false, Interval<short>.Bounded(0, true, 0, true) }
                };
            }
        }

        public static IEnumerable<object[]> CombineScenarios
        {
            get
            {
                return new object[][]
                {
                    new object[]
                    {
                        new Interval<double>[]
                        {
                            Interval<double>.LeftBounded(0, true),
                            Interval<double>.RightBounded(0, false)
                        },
                        Interval<double>.Unbounded()
                    },
                    new object[]
                    {
                        new Interval<double>[]
                        {
                            Interval<double>.Bounded(10, false, 15, false),
                            Interval<double>.Bounded(-5, false, 10, true),
                            Interval<double>.RightBounded(-5, true)
                        },
                        Interval<double>.RightBounded(15, false)
                    },
                    new object[]
                    {
                        new Interval<double>[]
                        {
                            Interval<double>.Bounded(-8, false, -2, true),
                            Interval<double>.LeftBounded(-2, false),
                            Interval<double>.Bounded(-9, true, -8, true)
                        },
                        Interval<double>.LeftBounded(-9, true)
                    },
                    new object[]
                    {
                        new Interval<double>[]
                        {
                            Interval<double>.Bounded(-5, true, -1, true),
                            Interval<double>.Bounded(-1, false, 1, false),
                            Interval<double>.Bounded(1, true, 5, false)
                        },
                        Interval<double>.Bounded(-5, true, 5, false)
                    },
                    new object[]
                    {
                        new Interval<double>[] { },
                        null
                    }
                };
            }
        }

        public static IEnumerable<object[]> CompareToScenarios
        {
            get
            {
                return new object[][]
                {
                    new object[]
                    {
                        Interval<double>.Unbounded(),
                        Interval<double>.Unbounded(),
                        0
                    },
                    new object[]
                    {
                        Interval<double>.Unbounded(),
                        null,
                        1
                    },
                    new object[]
                    {
                        Interval<double>.LeftBounded(-1, true),
                        Interval<double>.LeftBounded(-1, true),
                        0
                    },
                    new object[]
                    {
                        Interval<double>.LeftBounded(-1, true),
                        Interval<double>.LeftBounded(-1, false),
                        -1
                    },
                    new object[]
                    {
                        Interval<double>.RightBounded(1, true),
                        Interval<double>.RightBounded(1, true),
                        0
                    },
                    new object[]
                    {
                        Interval<double>.RightBounded(1, false),
                        Interval<double>.RightBounded(1, true),
                        1
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, true, 1, true),
                        Interval<double>.Bounded(-1, true, 1, true),
                        0
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, true, 1, true),
                        Interval<double>.Bounded(0, true, 1, true),
                        -1
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, true, 1, true),
                        Interval<double>.Bounded(-1, true, 0, true),
                        -1
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, false, 1, true),
                        Interval<double>.Bounded(-1, false, 1, true),
                        0
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, false, 1, false),
                        Interval<double>.Bounded(-1, false, 1, false),
                        0
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, false, 1, false),
                        Interval<double>.Bounded(-2, false, 1, false),
                        1
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, true, 1, false),
                        Interval<double>.Bounded(-1, true, 1, false),
                        0
                    }
                };
            }
        }

        public static IEnumerable<object[]> EqualsScenarios
        {
            get
            {
                return new object[][]
                {
                    new object[]
                    {
                        Interval<double>.Unbounded(),
                        Interval<double>.Unbounded(),
                        true
                    },
                    new object[]
                    {
                        Interval<double>.Unbounded(),
                        null,
                        false
                    },
                    new object[]
                    {
                        Interval<double>.LeftBounded(-1, true),
                        Interval<double>.LeftBounded(-1, true),
                        true
                    },
                    new object[]
                    {
                        Interval<double>.LeftBounded(-1, true),
                        Interval<double>.LeftBounded(-1, false),
                        false
                    },
                    new object[]
                    {
                        Interval<double>.RightBounded(1, true),
                        Interval<double>.RightBounded(1, true),
                        true
                    },
                    new object[]
                    {
                        Interval<double>.RightBounded(1, false),
                        Interval<double>.RightBounded(1, true),
                        false
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, true, 1, true),
                        Interval<double>.Bounded(-1, true, 1, true),
                        true
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, true, 1, true),
                        Interval<double>.Bounded(0, true, 1, true),
                        false
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, true, 1, true),
                        Interval<double>.Bounded(-1, true, 0, true),
                        false
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, false, 1, true),
                        Interval<double>.Bounded(-1, false, 1, true),
                        true
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, false, 1, false),
                        Interval<double>.Bounded(-1, false, 1, false),
                        true
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, false, 1, false),
                        Interval<double>.Bounded(-2, false, 1, false),
                        false
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, true, 1, false),
                        Interval<double>.Bounded(-1, true, 1, false),
                        true
                    }
                };
            }
        }

        public static IEnumerable<object[]> GapScenarios
        {
            get
            {
                return new object[][]
                {
                    new object[]
                    {
                        Interval<double>.LeftBounded(-1, true),
                        Interval<double>.Bounded(-4, true, -3, true),
                        Interval<double>.Bounded(-3, false, -1, false)
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, true, 1, true),
                        Interval<double>.RightBounded(-3, false),
                        Interval<double>.Bounded(-3, true, -1, false)
                    },
                    new object[]
                    {
                        Interval<double>.RightBounded(1, true),
                        Interval<double>.LeftBounded(3, true),
                        Interval<double>.Bounded(1, false, 3, false)
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, true, 1, true),
                        Interval<double>.Bounded(3, false, 4, true),
                        Interval<double>.Bounded(1, false, 3, true)
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, true, 0, false),
                        Interval<double>.Bounded(0, false, 1, true),
                        Interval<double>.Bounded(0, true, 0, true)
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, true, 1, true),
                        Interval<double>.Bounded(-3, true, -1, false),
                        null
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, true, 1, true),
                        Interval<double>.Bounded(-1, true, 1, true),
                        null
                    },
                    new object[]
                    {
                        Interval<double>.RightBounded(1, true),
                        Interval<double>.LeftBounded(1, false),
                        null
                    }
                };
            }
        }

        public static IEnumerable<object[]> GetHashCodeScenarios
        {
            get
            {
                return new object[][]
                {
                    new object[] { Interval<double>.Unbounded(), 0 },
                    new object[] { Interval<double>.LeftBounded(-1, true), -1074790399 },
                    new object[] { Interval<double>.RightBounded(1, true), 1072693250 },
                    new object[] { Interval<double>.Bounded(-1, true, 1, true), -2147483645 },
                    new object[] { Interval<double>.Bounded(-1, false, 1, true), -2147483646 },
                    new object[] { Interval<double>.Bounded(-1, false, 1, false), -2147483648 },
                    new object[] { Interval<double>.Bounded(-1, true, 1, false), -2147483647 }
                };
            }
        }

        public static IEnumerable<object[]> HasOverlapScenarios
        {
            get
            {
                return new object[][]
                {
                    new object[]
                    {
                        new Interval<double>[]
                        {
                            Interval<double>.Bounded(-1, true, 1, true),
                            null
                        },
                        false
                    },
                    new object[]
                    {
                        new Interval<double>[]
                        {
                            Interval<double>.Bounded(6, true, 10, true),
                            Interval<double>.Bounded(0, true, 5, true)
                        },
                        false
                    },
                    new object[]
                    {
                        new Interval<double>[]
                        {
                            Interval<double>.Bounded(12, true, 15, true),
                            Interval<double>.Bounded(5, true, 10, true),
                            Interval<double>.Bounded(0, true, 5, true)
                        },
                        true
                    }
                };
            }
        }

        public static IEnumerable<object[]> IncludesIntervalScenarios
        {
            get
            {
                return new object[][]
                {
                    new object[]
                    {
                        Interval<double>.Unbounded(),
                        null,
                        false
                    },
                    new object[]
                    {
                        Interval<double>.Unbounded(),
                        Interval<double>.Bounded(double.MinValue, true, double.MaxValue, true),
                        true
                    },
                    new object[]
                    {
                        Interval<double>.LeftBounded(-1, false),
                        Interval<double>.Bounded(-1, false, double.MaxValue, true),
                        true
                    },
                    new object[]
                    {
                        Interval<double>.RightBounded(1, false),
                        Interval<double>.Bounded(double.MinValue, true, 1, false),
                        true
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, true, 1, true),
                        Interval<double>.Bounded(-1.1, true, 0, true),
                        false
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, true, 1, true),
                        Interval<double>.Bounded(-1, true, 0, true),
                        true
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, true, 1, true),
                        Interval<double>.Bounded(-1, false, 0, true),
                        true
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, true, 1, true),
                        Interval<double>.Bounded(-1, true, 1, true),
                        true
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, true, 1, true),
                        Interval<double>.Bounded(-1, false, 1, false),
                        true
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, true, 1, true),
                        Interval<double>.Bounded(0, true, 1, false),
                        true
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, true, 1, true),
                        Interval<double>.Bounded(0, true, 1, true),
                        true
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, true, 1, true),
                        Interval<double>.Bounded(0, true, 1.1, true),
                        false
                    }
                };
            }
        }

        public static IEnumerable<object[]> IncludesValueScenarios
        {
            get
            {
                return new object[][]
                {
                    new object[] { Interval<double>.Unbounded(), double.MinValue, true },
                    new object[] { Interval<double>.Unbounded(), double.MaxValue, true },
                    new object[] { Interval<double>.LeftBounded(-1, true), double.MaxValue, true },
                    new object[] { Interval<double>.RightBounded(1, true), double.MinValue, true },
                    new object[] { Interval<double>.Bounded(-1, true, 1, true), -2, false },
                    new object[] { Interval<double>.Bounded(-1, true, 1, true), -1, true },
                    new object[] { Interval<double>.Bounded(-1, false, 1, true), -1, false },
                    new object[] { Interval<double>.Bounded(-1, true, 1, true), 0, true },
                    new object[] { Interval<double>.Bounded(-1, false, 1, false), 0, true },
                    new object[] { Interval<double>.Bounded(-1, true, 1, true), 1, true },
                    new object[] { Interval<double>.Bounded(-1, true, 1, false), 1, false },
                    new object[] { Interval<double>.Bounded(-1, true, 1, true), 2, false }
                };
            }
        }

        public static IEnumerable<object[]> IsContiguousScenarios
        {
            get
            {
                return new object[][]
                {
                    new object[]
                    {
                        new Interval<double>[]
                        {
                            Interval<double>.Bounded(0, true, 1, false),
                            Interval<double>.Unbounded(),
                            Interval<double>.Bounded(-5, true, 0, false)
                        },
                        false
                    },
                    new object[]
                    {
                        new Interval<double>[]
                        {
                            Interval<double>.Bounded(-10, false, 5, false),
                            null,
                            Interval<double>.LeftBounded(5, true)
                        },
                        false
                    },
                    new object[]
                    {
                        new Interval<double>[]
                        {
                            Interval<double>.LeftBounded(0, true),
                            Interval<double>.RightBounded(0, false)
                        },
                        true
                    },
                    new object[]
                    {
                        new Interval<double>[]
                        {
                            Interval<double>.Bounded(-1, false, 3, true),
                            Interval<double>.Bounded(3, false, 11, false),
                            Interval<double>.RightBounded(-1, true)
                        },
                        true
                    },
                    new object[]
                    {
                        new Interval<double>[]
                        {
                            Interval<double>.Bounded(-6, false, 0, true),
                            Interval<double>.Bounded(-8, true, -6, true),
                            Interval<double>.LeftBounded(0, false)
                        },
                        true
                    },
                    new object[]
                    {
                        new Interval<double>[]
                        {
                            Interval<double>.Bounded(-4, true, -2, false),
                            Interval<double>.Bounded(2, false, 4, true),
                            Interval<double>.Bounded(-2, true, 2, true)
                        },
                        true
                    },
                    new object[]
                    {
                        new Interval<double>[] { },
                        true
                    }
                };
            }
        }

        public static IEnumerable<object[]> IsEmptyScenarios
        {
            get
            {
                return new object[][]
                {
                    new object[] { 1, true, -1, true, true },
                    new object[] { 0, true, 0, false, true },
                    new object[] { 0, false, 0, true, true },
                    new object[] { 0, false, 0, false, true },
                    new object[] { -1, true, 1, true, false },
                    new object[] { -1, false, 1, false, false },
                    new object[] { 0, true, 0, true, false }
                };
            }
        }

        public static IEnumerable<object[]> IsPartitionedByScenarios
        {
            get
            {
                return new object[][]
                {
                    new object[]
                    {
                        Interval<double>.LeftBounded(-10, false),
                        new Interval<double>[]
                        {
                            Interval<double>.Bounded(-10, false, 5, false),
                            null,
                            Interval<double>.LeftBounded(5, true)
                        },
                        false
                    },
                    new object[]
                    {
                        Interval<double>.Unbounded(),
                        new Interval<double>[]
                        {
                            Interval<double>.LeftBounded(0, true),
                            Interval<double>.RightBounded(0, false)
                        },
                        true
                    },
                    new object[]
                    {
                        Interval<double>.RightBounded(11, false),
                        new Interval<double>[]
                        {
                            Interval<double>.Bounded(-1, false, 3, true),
                            Interval<double>.Bounded(3, false, 11, false),
                            Interval<double>.RightBounded(-1, true)
                        },
                        true
                    },
                    new object[]
                    {
                        Interval<double>.LeftBounded(-8, true),
                        new Interval<double>[]
                        {
                            Interval<double>.Bounded(-6, false, 0, true),
                            Interval<double>.Bounded(-8, true, -6, true),
                            Interval<double>.LeftBounded(0, false)
                        },
                        true
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-4, true, 4, true),
                        new Interval<double>[]
                        {
                            Interval<double>.Bounded(-4, true, -2, false),
                            Interval<double>.Bounded(2, false, 4, true),
                            Interval<double>.Bounded(-2, true, 2, true)
                        },
                        true
                    },
                    new object[]
                    {
                        Interval<double>.Unbounded(),
                        new Interval<double>[] { },
                        false
                    }
                };
            }
        }

        public static IEnumerable<object[]> OverlapsScenarios
        {
            get
            {
                return new object[][]
                {
                    new object[]
                    {
                        Interval<double>.Unbounded(),
                        Interval<double>.Unbounded(),
                        true
                    },
                    new object[]
                    {
                        Interval<double>.Unbounded(),
                        null,
                        false
                    },
                    new object[]
                    {
                        Interval<double>.LeftBounded(0, true),
                        Interval<double>.RightBounded(0, true),
                        true
                    },
                    new object[]
                    {
                        Interval<double>.LeftBounded(0, false),
                        Interval<double>.RightBounded(0, true),
                        false
                    },
                    new object[]
                    {
                        Interval<double>.RightBounded(0, true),
                        Interval<double>.LeftBounded(0, false),
                        false
                    },
                    new object[]
                    {
                        Interval<double>.RightBounded(1, false),
                        Interval<double>.RightBounded(1, true),
                        true
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, true, 1, true),
                        Interval<double>.Bounded(-1, true, 1, true),
                        true
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, false, 1, true),
                        Interval<double>.Bounded(-2, false, 0, true),
                        true
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, false, 1, false),
                        Interval<double>.Bounded(0, false, 2, false),
                        true
                    },
                    new object[]
                    {
                        Interval<double>.Bounded(-1, true, 1, false),
                        Interval<double>.Bounded(-2, true, 2, false),
                        true
                    }
                };
            }
        }

        public static IEnumerable<object[]> ToStringScenarios
        {
            get
            {
                return new object[][]
                {
                    new object[] { Interval<double>.Unbounded(), "(-∞,∞)" },
                    new object[] { Interval<double>.LeftBounded(-1, true), "[-1,∞)" },
                    new object[] { Interval<double>.RightBounded(1, true), "(-∞,1]" },
                    new object[] { Interval<double>.Bounded(-1, true, 1, true), "[-1,1]" },
                    new object[] { Interval<double>.Bounded(-1, false, 1, true), "(-1,1]" },
                    new object[] { Interval<double>.Bounded(-1, false, 1, false), "(-1,1)" },
                    new object[] { Interval<double>.Bounded(-1, true, 1, false), "[-1,1)" }
                };
            }
        }

        [Theory]
        [PropertyData("AbutsScenarios")]
        public void AbutsReturnsExpectedResult(Interval<double> sut, Interval<double> other, bool expected)
        {
            Assert.Equal(expected, sut.Abuts(other));
        }

        [Theory]
        [InlineData(-3, -2)]
        [InlineData(2, 3)]
        public void AbutsReturnsTrue(int left, int right)
        {
            var sut = Interval<int>.Bounded(-1, true, 1, true);
            var other = Interval<int>.Bounded(left, true, right, true);

            Assert.True(sut.Abuts(other));
        }

        [Theory]
        [PropertyData("BoundedIntScenarios")]
        public void BoundedIntReturnsExpectedResult(
            int left, bool isLeftClosed, int right, bool isRightClosed, Interval<int> expected)
        {
            var sut = Interval<int>.Bounded(left, isLeftClosed, right, isRightClosed);

            Assert.Equal(expected, sut);
        }

        [Theory]
        [PropertyData("BoundedLongScenarios")]
        public void BoundedLongReturnsExpectedResult(
            long left, bool isLeftClosed, long right, bool isRightClosed, Interval<long> expected)
        {
            var sut = Interval<long>.Bounded(left, isLeftClosed, right, isRightClosed);

            Assert.Equal(expected, sut);
        }

        [Theory]
        [InlineData(-3.4, true, -2.5, true)]
        [InlineData(1.0, true, 1.1, false)]
        [InlineData(-5.5, false, 6.2, true)]
        [InlineData(9.9, false, 11.2, false)]
        public void BoundedReturnsInitializedInstance(double left, bool isLeftClosed, double right, bool isRightClosed)
        {
            var sut = Interval<double>.Bounded(left, isLeftClosed, right, isRightClosed);

            Assert.Equal(left, sut.Left);
            Assert.Equal(isLeftClosed ? EndpointType.Closed : EndpointType.Open, sut.LeftType);
            Assert.Equal(right, sut.Right);
            Assert.Equal(isRightClosed ? EndpointType.Closed : EndpointType.Open, sut.RightType);
        }

        [Theory]
        [PropertyData("BoundedShortScenarios")]
        public void BoundedShortReturnsExpectedResult(
            short left, bool isLeftClosed, short right, bool isRightClosed, Interval<short> expected)
        {
            var sut = Interval<short>.Bounded(left, isLeftClosed, right, isRightClosed);

            Assert.Equal(expected, sut);
        }

        [Theory]
        [PropertyData("BoundedEmptyIntervalScenarios")]
        public void BoundedWithEmptyIntervalThrowsArgumentException(
            double left, bool isLeftClosed, double right, bool isRightClosed)
        {
            var ex = Assert.Throws<ArgumentException>(
                () => Interval<double>.Bounded(left, isLeftClosed, right, isRightClosed));

            Assert.NotNull(ex.Message);
        }

        [Fact]
        public void CombineNonContiguousIntervalsThrowsArgumentException()
        {
            var intervals = new List<Interval<double>>();
            intervals.Add(Interval<double>.Bounded(2, true, 5, true));
            intervals.Add(Interval<double>.Bounded(0, true, 1, true));
            intervals.Add(Interval<double>.Bounded(-5, true, 0, true));

            var ex = Assert.Throws<ArgumentException>(() => Interval<double>.Combine(intervals));

            Assert.Equal("intervals", ex.ParamName);
            Assert.NotNull(ex.Message);
        }

        [Fact]
        public void CombineNullIntervalsThrowsArgumentNullException()
        {
            List<Interval<double>> nullIntervals = null;

            var ex = Assert.Throws<ArgumentNullException>(() => Interval<double>.Combine(nullIntervals));

            Assert.Equal("intervals", ex.ParamName);
        }

        [Theory]
        [PropertyData("CombineScenarios")]
        public void CombineReturnsExpectedResult(IEnumerable<Interval<double>> intervals, Interval<double> expected)
        {
            Assert.Equal(expected, Interval<double>.Combine(intervals));
        }

        [Theory]
        [PropertyData("CompareToScenarios")]
        public void CompareToReturnsExpectedResult(Interval<double> sut, Interval<double> other, int expected)
        {
            Assert.Equal(expected, sut.CompareTo(other));
            Assert.Equal(expected, sut.CompareTo((object)other));
            Assert.Equal(expected < 0, sut < other);
            Assert.Equal(expected <= 0, sut <= other);
            Assert.Equal(expected > 0, sut > other);
            Assert.Equal(expected >= 0, sut >= other);
        }

        [Theory]
        [PropertyData("EqualsScenarios")]
        public void EqualsReturnsExpectedResult(Interval<double> sut, Interval<double> other, bool expected)
        {
            Assert.Equal(expected, sut.Equals(other));
            Assert.Equal(expected, sut.Equals((object)other));
            Assert.Equal(expected, sut == other);
            Assert.Equal(!expected, sut != other);
        }

        [Theory]
        [PropertyData("GapScenarios")]
        public void GapReturnsExpectedResult(Interval<double> sut, Interval<double> other, Interval<double> expected)
        {
            Assert.Equal(expected, sut.Gap(other));
        }

        [Theory]
        [InlineData(-3, -2)]
        [InlineData(2, 3)]
        public void GapReturnsNull(int left, int right)
        {
            var sut = Interval<int>.Bounded(-1, true, 1, true);
            var other = Interval<int>.Bounded(left, true, right, true);

            Assert.Equal(null, sut.Gap(other));
        }

        [Theory]
        [PropertyData("GetHashCodeScenarios")]
        public void GetHashCodeReturnsExpectedResult(Interval<double> sut, int expected)
        {
            Assert.Equal(expected, sut.GetHashCode());
        }

        [Theory]
        [PropertyData("HasOverlapScenarios")]
        public void HasOverlapReturnsExpectedResult(IEnumerable<Interval<double>> intervals, bool expected)
        {
            Assert.Equal(expected, Interval<double>.HasOverlap(intervals));
        }

        [Fact]
        public void HasOverlapWithNullIntervalsThrowsArgumentNullException()
        {
            List<Interval<double>> nullIntervals = null;

            var ex = Assert.Throws<ArgumentNullException>(() => Interval<double>.HasOverlap(nullIntervals));

            Assert.Equal("intervals", ex.ParamName);
        }

        [Theory]
        [PropertyData("IncludesIntervalScenarios")]
        public void IncludesIntervalReturnsExpectedResult(
            Interval<double> sut, Interval<double> other, bool expected)
        {
            Assert.Equal(expected, sut.Includes(other));
        }

        [Theory]
        [PropertyData("IncludesValueScenarios")]
        public void IncludesValueReturnsExpectedResult(Interval<double> sut, double value, bool expected)
        {
            Assert.Equal(expected, sut.Includes(value));
        }

        [Theory]
        [PropertyData("IsContiguousScenarios")]
        public void IsContiguousReturnsExpectedResult(IEnumerable<Interval<double>> intervals, bool expected)
        {
            Assert.Equal(expected, Interval<double>.IsContiguous(intervals));
        }

        [Fact]
        public void IsContiguousWithNullIntervalsThrowsArgumentNullException()
        {
            List<Interval<double>> nullIntervals = null;

            var ex = Assert.Throws<ArgumentNullException>(() => Interval<double>.IsContiguous(nullIntervals));

            Assert.Equal("intervals", ex.ParamName);
        }

        [Theory]
        [PropertyData("IsEmptyScenarios")]
        public void IsEmptyReturnsExpectedResult(
            double left, bool isLeftClosed, double right, bool isRightClosed, bool expected)
        {
            Assert.Equal(expected, Interval<double>.IsEmpty(left, isLeftClosed, right, isRightClosed));
        }

        [Fact]
        public void IsPartitionedByNullIntervalsThrowsArgumentNullException()
        {
            var sut = Interval<double>.Unbounded();
            List<Interval<double>> nullIntervals = null;

            var ex = Assert.Throws<ArgumentNullException>(() => sut.IsPartitionedBy(nullIntervals));

            Assert.Equal("intervals", ex.ParamName);
        }

        [Theory]
        [PropertyData("IsPartitionedByScenarios")]
        public void IsPartitionedByReturnsExpectedResult(
            Interval<double> sut, IEnumerable<Interval<double>> intervals, bool expected)
        {
            Assert.Equal(expected, sut.IsPartitionedBy(intervals));
        }

        [Theory]
        [InlineData(-10.1, true)]
        [InlineData(13.2, false)]
        public void LeftBoundedReturnsInitializedInstance(double left, bool isLeftClosed)
        {
            var sut = Interval<double>.LeftBounded(left, isLeftClosed);

            Assert.Equal(left, sut.Left);
            Assert.Equal(isLeftClosed ? EndpointType.Closed : EndpointType.Open, sut.LeftType);
            Assert.Equal(null, sut.Right);
            Assert.Equal(EndpointType.Unbounded, sut.RightType);
        }

        [Theory]
        [PropertyData("OverlapsScenarios")]
        public void OverlapsReturnsExpectedResult(Interval<double> sut, Interval<double> other, bool expected)
        {
            Assert.Equal(expected, sut.Overlaps(other));
        }

        [Theory]
        [InlineData(8.9, true)]
        [InlineData(-17.6, false)]
        public void RightBoundedReturnsInitializedInstance(double right, bool isRightClosed)
        {
            var sut = Interval<double>.RightBounded(right, isRightClosed);

            Assert.Equal(null, sut.Left);
            Assert.Equal(EndpointType.Unbounded, sut.LeftType);
            Assert.Equal(right, sut.Right);
            Assert.Equal(isRightClosed ? EndpointType.Closed : EndpointType.Open, sut.RightType);
        }

        [Theory]
        [PropertyData("ToStringScenarios")]
        public void ToStringReturnsExpectedResult(Interval<double> sut, string expected)
        {
            Assert.Equal(expected, sut.ToString());
        }

        [Fact]
        public void UnboundedReturnsInitializedInstance()
        {
            var sut = Interval<double>.Unbounded();

            Assert.Equal(null, sut.Left);
            Assert.Equal(EndpointType.Unbounded, sut.LeftType);
            Assert.Equal(null, sut.Right);
            Assert.Equal(EndpointType.Unbounded, sut.RightType);
        }
    }
}
