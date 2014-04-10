namespace Georadix.Core
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides a generic <see cref="IEqualityComparer{T}"/> implementation.
    /// </summary>
    /// <typeparam name="T">The type of object to compare.</typeparam>
    public class DelegateEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> comparer;
        private readonly Func<T, int> hash;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateEqualityComparer{T}"/> class.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        /// <param name="hash">The hash.</param>
        public DelegateEqualityComparer(Func<T, T, bool> comparer, Func<T, int> hash)
        {
            comparer.AssertNotNull("comparer");
            hash.AssertNotNull("hash");

            this.comparer = comparer;
            this.hash = hash;
        }

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type {T} to compare.</param>
        /// <param name="y">The second object of type {T} to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        public bool Equals(T x, T y)
        {
            return this.comparer(x, y);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures 
        /// like a hash table. 
        /// </returns>
        public int GetHashCode(T obj)
        {
            return this.hash(obj);
        }
    }
}
