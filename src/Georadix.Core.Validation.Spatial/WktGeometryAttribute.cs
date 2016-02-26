namespace Georadix.Core.Validation.Spatial
{
    using GeoAPI.Geometries;
    using NetTopologySuite.IO;
    using System;
    using System.ComponentModel.DataAnnotations;
    using ParseException = GeoAPI.IO.ParseException;

    /// <summary>
    /// Represents an attribute used to validate geometries in the well-known text (WKT) format.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class WktGeometryAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WktGeometryAttribute"/> class.
        /// </summary>
        /// <param name="allowedTypes">The allowed geometry types.</param>
        public WktGeometryAttribute(GeometryFlags allowedTypes = GeometryFlags.Default)
        {
            this.AllowedTypes = allowedTypes;
        }

        /// <summary>
        /// Gets or sets the allowed geometry types.
        /// </summary>
        public GeometryFlags AllowedTypes { get; set; }

        /// <summary>
        /// Determines whether the specified value of the object is valid.
        /// </summary>
        /// <param name="value">The value of the object to validate.</param>
        /// <returns>
        /// <c>true</c> if the specified value is valid; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsValid(object value)
        {
            var stringValue = value as string;

            if (!string.IsNullOrEmpty(stringValue))
            {
                try
                {
                    var geometry = new WKTReader().Read(stringValue);

                    if (geometry.IsEmpty || !geometry.IsValid)
                    {
                        return false;
                    }

                    if (this.AllowedTypes == GeometryFlags.Default)
                    {
                        return true;
                    }

                    return
                        (this.AllowedTypes.HasFlag(GeometryFlags.GeometryCollection) &&
                        (geometry is IGeometryCollection)) ||
                        (this.AllowedTypes.HasFlag(GeometryFlags.LineString) &&
                        (geometry is ILineString)) ||
                        (this.AllowedTypes.HasFlag(GeometryFlags.MultiLineString) &&
                        (geometry is IMultiLineString)) ||
                        (this.AllowedTypes.HasFlag(GeometryFlags.MultiPoint) &&
                        (geometry is IMultiPoint)) ||
                        (this.AllowedTypes.HasFlag(GeometryFlags.MultiPolygon) &&
                        (geometry is IMultiPolygon)) ||
                        (this.AllowedTypes.HasFlag(GeometryFlags.Point) &&
                        (geometry is IPoint)) ||
                        (this.AllowedTypes.HasFlag(GeometryFlags.Polygon) &&
                        (geometry is IPolygon));
                }
                catch (ParseException)
                {
                    return false;
                }
            }

            return true;
        }
    }
}