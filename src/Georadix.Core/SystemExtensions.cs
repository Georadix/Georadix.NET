﻿namespace System
{
    using Georadix.Core;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Defines methods that extend system types.
    /// </summary>
    public static class SystemExtensions
    {
        #region Types

        private static readonly Func<MethodInfo, IEnumerable<Type>> ParameterTypeProjection =
            method => method.GetParameters().Select(p => p.ParameterType.GetGenericTypeDefinition());

        /// <summary>
        /// Gets the generic method.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <param name="parameterTypes">The parameter types.</param>
        /// <returns>A <see cref="MethodInfo"/> instance representing the generic method.</returns>
        public static MethodInfo GetGenericMethod(this Type type, string name, params Type[] parameterTypes)
        {
            return (from method in type.GetMethods()
                    where method.Name == name
                    where parameterTypes.SequenceEqual(ParameterTypeProjection(method))
                    select method).SingleOrDefault();
        }

        #endregion

        #region Numeric Types

        /// <summary>
        /// Clips the number to the specified minimum and maximum values.
        /// </summary>
        /// <param name="value">The number to clip.</param>
        /// <param name="min">The minimum allowable value.</param>
        /// <param name="max">The maximum allowable value.</param>
        /// <returns>The clipped value.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="min"/> is greater than <paramref name="max"/>.
        /// </exception>
        public static short Clip(this short value, short min, short max)
        {
            min.AssertLessThanOrEqualTo(max, "min");

            return Math.Min(Math.Max(value, min), max);
        }

        /// <summary>
        /// Clips the number to the specified minimum and maximum values.
        /// </summary>
        /// <param name="value">The number to clip.</param>
        /// <param name="min">The minimum allowable value.</param>
        /// <param name="max">The maximum allowable value.</param>
        /// <returns>The clipped value.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="min"/> is greater than <paramref name="max"/>.
        /// </exception>
        public static int Clip(this int value, int min, int max)
        {
            min.AssertLessThanOrEqualTo(max, "min");

            return Math.Min(Math.Max(value, min), max);
        }

        /// <summary>
        /// Clips the number to the specified minimum and maximum values.
        /// </summary>
        /// <param name="value">The number to clip.</param>
        /// <param name="min">The minimum allowable value.</param>
        /// <param name="max">The maximum allowable value.</param>
        /// <returns>The clipped value.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="min"/> is greater than <paramref name="max"/>.
        /// </exception>
        public static long Clip(this long value, long min, long max)
        {
            min.AssertLessThanOrEqualTo(max, "min");

            return Math.Min(Math.Max(value, min), max);
        }

        /// <summary>
        /// Clips the number to the specified minimum and maximum values.
        /// </summary>
        /// <param name="value">The number to clip.</param>
        /// <param name="min">The minimum allowable value.</param>
        /// <param name="max">The maximum allowable value.</param>
        /// <returns>The clipped value.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="min"/> is greater than <paramref name="max"/>.
        /// </exception>
        public static float Clip(this float value, float min, float max)
        {
            min.AssertLessThanOrEqualTo(max, "min");

            return Math.Min(Math.Max(value, min), max);
        }

        /// <summary>
        /// Clips the number to the specified minimum and maximum values.
        /// </summary>
        /// <param name="value">The number to clip.</param>
        /// <param name="min">The minimum allowable value.</param>
        /// <param name="max">The maximum allowable value.</param>
        /// <returns>The clipped value.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="min"/> is greater than <paramref name="max"/>.
        /// </exception>
        public static double Clip(this double value, double min, double max)
        {
            min.AssertLessThanOrEqualTo(max, "min");

            return Math.Min(Math.Max(value, min), max);
        }

        /// <summary>
        /// Returns a value indicating whether this instance and a specified <see cref="Single"/> represent the same
        /// value based on a specified tolerance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="value">The value to compare to the instance.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <returns><c>true</c> if they are equal; otherwise, <c>false</c>.</returns>
        public static bool IsEqualTo(this float instance, float value, float tolerance)
        {
            return Math.Abs(instance - value) <= tolerance;
        }

        /// <summary>
        /// Returns a value indicating whether this instance and a specified <see cref="Double"/> represent the same
        /// value based on a specified tolerance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="value">The value to compare to the instance.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <returns><c>true</c> if they are equal; otherwise, <c>false</c>.</returns>
        public static bool IsEqualTo(this double instance, double value, double tolerance)
        {
            return Math.Abs(instance - value) <= tolerance;
        }

        #endregion Numeric Types

        #region Event Handler Types

        /// <summary>
        /// Raises the event.
        /// </summary>
        /// <param name="handler">The event handler.</param>
        /// <param name="sender">The source of the event.</param>
        public static void Raise(this EventHandler handler, object sender)
        {
            if (handler != null)
            {
                handler(sender, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the event.
        /// </summary>
        /// <typeparam name="T">An event args class.</typeparam>
        /// <param name="handler">The event handler.</param>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        public static void Raise<T>(this EventHandler<T> handler, object sender, T e)
            where T : EventArgs
        {
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        #endregion Event Handler Types
    }
}