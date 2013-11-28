namespace System.Linq
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines methods that extend LINQ.
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// Adds the specified elements to the end of the collection.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="items">The items to add.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> is <see langword="null"/>.
        /// </exception>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            collection.AssertNotNull("collection");

            if (items != null)
            {
                items.ForEach(i => collection.Add(i));
            }
        }

        /// <summary>
        /// Executes a specified action on all elements in the collection.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="action">The action.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> is <see langword="null"/>.
        /// </exception>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            collection.AssertNotNull("collection");

            foreach (var item in collection)
            {
                action(item);
            }
        }

        /// <summary>
        /// Returns the maximal element of a sequence based on a specified projection.
        /// </summary>
        /// <remarks>
        /// If more than one element has the maximal projected value, the first one encountered will be returned. This
        /// overload uses the default comparer for the projected type. This operator uses immediate execution, but only
        /// buffers a single result (the current maximal element).
        /// </remarks>
        /// <typeparam name="TSource">The type of source sequence.</typeparam>
        /// <typeparam name="TKey">The type of projected element.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The maximal element, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="selector"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty.</exception>
        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            return source.MaxBy(selector, Comparer<TKey>.Default);
        }

        /// <summary>
        /// Returns the maximal element of a sequence based on a specified projection and comparer for projected
        /// values.
        /// </summary>
        /// <remarks>
        /// If more than one element has the maximal projected value, the first one encountered will be returned. This
        /// overload uses the default comparer for the projected type. This operator uses immediate execution, but only
        /// buffers a single result (the current maximal element).
        /// </remarks>
        /// <typeparam name="TSource">The type of source sequence.</typeparam>
        /// <typeparam name="TKey">The type of projected element.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <param name="comparer">A comparer used to compare projected values.</param>
        /// <returns>The maximal element, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="selector"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty.</exception>
        public static TSource MaxBy<TSource, TKey>(
            this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            source.AssertNotNull("source");
            selector.AssertNotNull("selector");
            comparer.AssertNotNull("comparer");

            using (IEnumerator<TSource> sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("The sequence is empty.");
                }

                var max = sourceIterator.Current;
                var maxKey = selector(max);

                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = selector(candidate);

                    if (comparer.Compare(candidateProjected, maxKey) > 0)
                    {
                        max = candidate;
                        maxKey = candidateProjected;
                    }
                }

                return max;
            }
        }

        /// <summary>
        /// Returns the minimal element of a sequence based on a specified projection.
        /// </summary>
        /// <remarks>
        /// If more than one element has the minimal projected value, the first one encountered will be returned. This
        /// overload uses the default comparer for the projected type. This operator uses immediate execution, but only
        /// buffers a single result (the current minimal element).
        /// </remarks>
        /// <typeparam name="TSource">The type of source sequence.</typeparam>
        /// <typeparam name="TKey">The type of projected element.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The minimal element, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="selector"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty.</exception>
        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            return source.MinBy(selector, Comparer<TKey>.Default);
        }

        /// <summary>
        /// Returns the minimal element of a sequence based on a specified projection and comparer for projected
        /// values.
        /// </summary>
        /// <remarks>
        /// If more than one element has the minimal projected value, the first one encountered will be returned. This
        /// overload uses the default comparer for the projected type. This operator uses immediate execution, but only
        /// buffers a single result (the current minimal element).
        /// </remarks>
        /// <typeparam name="TSource">The type of source sequence</typeparam>
        /// <typeparam name="TKey">The type of projected element.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <param name="comparer">A comparer used to compare projected values.</param>
        /// <returns>The minimal element, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="selector"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty.</exception>
        public static TSource MinBy<TSource, TKey>(
            this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            source.AssertNotNull("source");
            selector.AssertNotNull("selector");
            comparer.AssertNotNull("comparer");

            using (IEnumerator<TSource> sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("The sequence is empty.");
                }

                var min = sourceIterator.Current;
                var minKey = selector(min);

                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = selector(candidate);

                    if (comparer.Compare(candidateProjected, minKey) < 0)
                    {
                        min = candidate;
                        minKey = candidateProjected;
                    }
                }

                return min;
            }
        }
    }
}