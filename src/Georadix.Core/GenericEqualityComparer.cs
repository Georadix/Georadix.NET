namespace Georadix.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
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
            if (x == null)
            {
                return (y != null) ? false : true;
            }

            if (y == null)
            {
                return false;
            }

            var type = typeof(T);
            var enumerableType = (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>)) ?
                type :
                type.GetInterfaces().SingleOrDefault(
                    i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            if ((type == typeof(string)) || type.IsValueType)
            {
                // If dealing with strings or value types (including enums), compare them directly.
                return x.Equals(y);
            }
            else if (enumerableType != null)
            {
                // If dealing with enumerables, compare their items taking the order into account.
                var itemType = enumerableType.GetGenericArguments()[0];
                var comparerType = typeof(GenericEqualityComparer<>).MakeGenericType(itemType);
                dynamic comparer = Activator.CreateInstance(comparerType);

                var itemsX = ((IEnumerable)x).Cast<object>();
                var itemsY = ((IEnumerable)y).Cast<object>();

                if (itemsX.Count() != itemsY.Count())
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i < itemsX.Count(); i++)
                    {
                        dynamic itemX = itemsX.ElementAt(i);
                        dynamic itemY = itemsY.ElementAt(i);

                        if (!comparer.Equals(itemX, itemY))
                        {
                            return false;
                        }
                    }
                }
            }
            else
            {
                // For all other classes, compare their properties.
                foreach (var property in this.properties)
                {
                    dynamic valueX = property.GetValue(x);
                    dynamic valueY = property.GetValue(y);

                    var comparerType = typeof(GenericEqualityComparer<>).MakeGenericType(property.PropertyType);
                    dynamic comparer = Activator.CreateInstance(comparerType);

                    if (!comparer.Equals(valueX, valueY))
                    {
                        return false;
                    }
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