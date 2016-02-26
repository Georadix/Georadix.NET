namespace Georadix.Core.Validation.Spatial
{
    using System;

    /// <summary>
    /// Defines flags that control the types of geometries and the way validation is performed on spatial properties.
    /// </summary>
    [Flags]
    public enum GeometryFlags
    {
        /// <summary>
        /// Any geometry.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Point geometry.
        /// </summary>
        Point = 1,

        /// <summary>
        /// Multi point geometry.
        /// </summary>
        MultiPoint = 2,

        /// <summary>
        /// Line string geometry.
        /// </summary>
        LineString = 4,

        /// <summary>
        /// Multi line string geometry.
        /// </summary>
        MultiLineString = 8,

        /// <summary>
        /// Polygon geometry.
        /// </summary>
        Polygon = 16,

        /// <summary>
        /// Multi polygon geometry.
        /// </summary>
        MultiPolygon = 32,

        /// <summary>
        /// Geometry collection.
        /// </summary>
        GeometryCollection = 64
    }
}