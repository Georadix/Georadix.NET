namespace Georadix.Core.Validation.Spatial
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Xunit;
    using Xunit.Extensions;

    public class WktGeometryAttributeFixture
    {
        public static IEnumerable<object[]> GeometryScenarios
        {
            get
            {
                return new object[][]
                {
                    new object[]
                    {
                        GeometryFlags.Default,
                        null,
                        true
                    },
                    new object[]
                    {
                        GeometryFlags.LineString,
                        "not-a-geometry",
                        false
                    },
                    new object[]
                    {
                        GeometryFlags.Point,
                        "POINT EMPTY",
                        false
                    },
                    new object[]
                    {
                        GeometryFlags.Polygon,
                        "POLYGON((1 1, -1 1, 1 -1, -1 -1, 1 1))",
                        false
                    },
                    new object[]
                    {
                        GeometryFlags.Default,
                        "POINT(1 1 80)",
                        true
                    },
                    new object[]
                    {
                        GeometryFlags.GeometryCollection,
                        "GEOMETRYCOLLECTION(POINT(4 6),LINESTRING(4 6,7 10))",
                        true
                    },
                    new object[]
                    {
                        GeometryFlags.GeometryCollection,
                        "POINT(1 1 80)",
                        false
                    },
                    new object[]
                    {
                        GeometryFlags.LineString,
                        "LINESTRING(30 10, 10 30, 40 40)",
                        true
                    },
                    new object[]
                    {
                        GeometryFlags.LineString,
                        "GEOMETRYCOLLECTION(POINT(4 6),LINESTRING(4 6,7 10))",
                        false
                    },
                    new object[]
                    {
                        GeometryFlags.MultiLineString,
                        "MULTILINESTRING((10 10, 20 20, 10 40),(40 40, 30 30, 40 20, 30 10))",
                        true
                    },
                    new object[]
                    {
                        GeometryFlags.MultiLineString,
                        "LINESTRING(30 10, 10 30, 40 40)",
                        false
                    },
                    new object[]
                    {
                        GeometryFlags.MultiPoint,
                        "MULTIPOINT(10 40, 40 30, 20 20, 30 10)",
                        true
                    },
                    new object[]
                    {
                        GeometryFlags.MultiPoint,
                        "MULTILINESTRING((10 10, 20 20, 10 40),(40 40, 30 30, 40 20, 30 10))",
                        false
                    },
                    new object[]
                    {
                        GeometryFlags.MultiPolygon,
                        "MULTIPOLYGON(((30 20, 45 40, 10 40, 30 20)),((15 5, 40 10, 10 20, 5 10, 15 5)))",
                        true
                    },
                    new object[]
                    {
                        GeometryFlags.MultiPolygon,
                        "MULTIPOINT(10 40, 40 30, 20 20, 30 10)",
                        false
                    },
                    new object[]
                    {
                        GeometryFlags.Point,
                        "POINT(30 10)",
                        true
                    },
                    new object[]
                    {
                        GeometryFlags.Point,
                        "MULTIPOLYGON(((30 20, 45 40, 10 40, 30 20)),((15 5, 40 10, 10 20, 5 10, 15 5)))",
                        false
                    },
                    new object[]
                    {
                        GeometryFlags.Polygon,
                        "POLYGON((35 10, 45 45, 15 40, 10 20, 35 10),(20 30, 35 35, 30 20, 20 30))",
                        true
                    },
                    new object[]
                    {
                        GeometryFlags.Polygon,
                        "POINT(30 10)",
                        false
                    },
                    new object[]
                    {
                        GeometryFlags.Polygon | GeometryFlags.MultiPolygon,
                        "POLYGON((30 10, 40 40, 20 40, 10 20, 30 10))",
                        true
                    },
                    new object[]
                    {
                        GeometryFlags.Polygon | GeometryFlags.MultiPolygon,
                        "MULTIPOLYGON(((40 40, 20 45, 45 30, 40 40)),((20 35, 10 30, 10 10, 30 5, 45 20, 20 35),(30 20, 20 15, 20 25, 30 20)))",
                        true
                    },
                    new object[]
                    {
                        GeometryFlags.Polygon | GeometryFlags.MultiPolygon,
                        "POINT(30 10)",
                        false
                    }
                };
            }
        }

        [Theory]
        [MemberData("GeometryScenarios")]
        public void ValidateGeometryReturnsExpectedResult(GeometryFlags flags, string value, bool expected)
        {
            var sut = new WktGeometryAttribute(flags);

            Assert.Equal(expected, sut.IsValid(value));
        }
    }
}