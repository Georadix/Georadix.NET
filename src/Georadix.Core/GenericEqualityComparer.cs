namespace Georadix.Core
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Provides an <see cref="IEqualityComparer{T}"/> implementation that recursively compares all the properties of
    /// an object.
    /// </summary>
    /// <typeparam name="T">The type of object to compare.</typeparam>
    public class GenericEqualityComparer<T> : IEqualityComparer<T>
    {
        private List<PropertyInfo> properties = new List<PropertyInfo>();

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericEqualityComparer{T}"/> class.
        /// </summary>
        public GenericEqualityComparer()
        {
            this.properties.AddRange(
                typeof(T).GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public));
        }

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// <c>true</c> if the specified objects are equal; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(T x, T y)
        {
            foreach (var property in this.properties)
            {
                dynamic valueX = property.GetValue(x);
                dynamic valueY = property.GetValue(y);

                if ((valueX == null) || (valueY == null))
                {
                    if (valueX != valueY)
                    {
                        return false;
                    }
                }
                else if (property.PropertyType.IsClass && (property.PropertyType != typeof(string)))
                {
                    var comparerType = typeof(GenericEqualityComparer<>).MakeGenericType(property.PropertyType);
                    dynamic comparer = Activator.CreateInstance(comparerType);

                    if (!comparer.Equals(valueX, valueY))
                    {
                        return false;
                    }
                }
                else if (!valueX.Equals(valueY))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash
        /// table.
        /// </returns>
        public int GetHashCode(T obj)
        {
            return (obj == null) ? 0 : obj.GetHashCode();
        }
    }
}