﻿namespace Georadix.Core.Validation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    /// <summary>
    /// Extensions related to data annotations and validation.
    /// </summary>
    public static class DataAnnotationExtensions
    {
        /// <summary>
        /// Ensures the object is valid by validating all data annotations.
        /// </summary>
        /// <param name="subject">The object to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="items">
        /// A dictionary of key/value pairs to make available data annotation attributes. This parameter is optional.
        /// </param>
        /// <param name="serviceProvider">
        /// The service provider to allow data annotation attributes to resolve additional dependencies. This
        /// parameter is optional.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Contains the error message of all <see cref="ValidationResult"/>.
        /// </exception>
        public static void AssertValid(
            this object subject,
            string paramName,
            IDictionary<object, object> items = null,
            IServiceProvider serviceProvider = null)
        {
            var validationResults = subject.Validate(items, serviceProvider);

            if (validationResults.Any())
            {
                var errorMessages = new List<string>();

                foreach (var result in validationResults)
                {
                    errorMessages.Add(
                        string.Format("{0}: {1}", string.Join(", ", result.MemberNames), result.ErrorMessage));
                }

                throw new ArgumentException(string.Join(Environment.NewLine, errorMessages), paramName);
            }
        }

        /// <summary>
        /// Validates an object instance using the data annotations validator.
        /// </summary>
        /// <param name="subject">The object instance to validate.</param>
        /// <param name="items">
        /// A dictionary of key/value pairs to make available data annotation attributes. This parameter is optional.
        /// </param>
        /// <param name="serviceProvider">
        /// The service provider to allow data annotation attributes to resolve additional dependencies. This
        /// parameter is optional.
        /// </param>
        /// <returns>
        /// A list of <see cref="ValidationResult"/> containing validation errors. The list will be empty if the
        /// model is valid.
        /// </returns>
        public static IEnumerable<ValidationResult> Validate(
            this object subject,
            IDictionary<object, object> items = null,
            IServiceProvider serviceProvider = null)
        {
            var context = new ValidationContext(subject, serviceProvider, items);

            var validationResults = new List<ValidationResult>();

            Validator.TryValidateObject(subject, context, validationResults, true);

            var properties = subject.GetType().GetProperties().Where(prop => prop.CanRead).ToList();

            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string) || property.PropertyType.IsValueType)
                {
                    continue;
                }

                var value = subject.GetType().GetProperty(property.Name).GetValue(subject);

                if (value == null)
                {
                    continue;
                }

                var asEnumerable = value as IEnumerable;

                if (asEnumerable != null)
                {
                    int index = 0;

                    foreach (var enumObj in asEnumerable)
                    {
                        var nestedResults = enumObj.Validate(items, serviceProvider);

                        foreach (var validationResult in nestedResults)
                        {
                            validationResults.Add(new ValidationResult(
                                validationResult.ErrorMessage,
                                validationResult.MemberNames.Select(x => string.Format(
                                    "{0}[{1}].{2}",
                                    property.Name,
                                    index++,
                                    x))));
                        }
                    }
                }
                else
                {
                    var nestedResults = value.Validate(items, serviceProvider);

                    foreach (var validationResult in nestedResults)
                    {
                        validationResults.Add(new ValidationResult(
                            validationResult.ErrorMessage,
                            validationResult.MemberNames.Select(x => property.Name + '.' + x)));
                    }
                }
            }

            return validationResults;
        }
    }
}