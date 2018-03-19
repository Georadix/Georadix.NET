namespace Georadix.Core
{
    using System;

    /// <summary>
    /// Helpers for enumerations.
    /// </summary>
    public class EnumHelper
    {
        /// <summary>
        /// Maps the specified source value to a given enumeration type by the Name of the enumeration value.
        /// The source is passed as an <see cref="Enum"/> to avoid requiring both types to be specified.
        /// </summary>
        /// <typeparam name="TEnumTarget">The type of the enumeration target.</typeparam>
        /// <param name="source">The source.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> does not exist on the target enumeration.
        /// </exception>
        /// <returns>An enumeration value of the specified target type.</returns>
        public static TEnumTarget Map<TEnumTarget>(Enum source)
            where TEnumTarget : struct
        {
            source.AssertNotNull("source");

            var value = source.ToString();

            if (!Enum.TryParse<TEnumTarget>(value, out TEnumTarget result))
            {
                throw new ArgumentException(
                    string.Format(
                        "The value '{0}' does not exist on the '{1}' enumeration.",
                        value,
                        typeof(TEnumTarget).FullName),
                    "source");
            }

            return result;
        }

        /// <summary>
        /// Maps the specified source value to a given enumeration type by the Name of the enumeration value.
        /// The source is passed as a nullable <see cref="Enum"/> to avoid requiring both types to be specified.
        /// </summary>
        /// <typeparam name="TEnumTarget">The type of the enumeration target.</typeparam>
        /// <param name="source">The source.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> does not exist on the target enumeration.
        /// </exception>
        /// <returns>A nullable enumeration value of the specified target type.</returns>
        public static TEnumTarget? MapNullable<TEnumTarget>(Enum source)
            where TEnumTarget : struct
        {
            if (source != null)
            {
                return Map<TEnumTarget>(source);
            }

            return default(TEnumTarget?);
        }
    }
}
