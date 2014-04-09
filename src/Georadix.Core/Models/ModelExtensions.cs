namespace Georadix.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    /// <summary>
    /// Extensions for classes marked with the <see cref="IModel"/> interface.
    /// </summary>
    public static class ModelExtensions
    {
        /// <summary>
        /// Validates a model instance using the data annotations validator.
        /// </summary>
        /// <param name="model">
        /// The model instance to validate
        /// </param>
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
        public static IEnumerable<ValidationResult> Validate(this IModel model, IDictionary<object, object> items = null, IServiceProvider serviceProvider = null)
        {
            var context = new ValidationContext(model, serviceProvider, items);

            var validationResults = new List<ValidationResult>();

            Validator.TryValidateObject(model, context, validationResults);

            return validationResults;
        }

        /// <summary>
        /// Ensures the model is valid by validating all data annotations.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="items">
        /// A dictionary of key/value pairs to make available data annotation attributes. This parameter is optional.
        /// </param>
        /// <param name="serviceProvider">
        /// The service provider to allow data annotation attributes to resolve additional dependencies. This 
        /// parameter is optional.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Contains the error message of the first <see cref="ValidationResult"/>.
        /// </exception>
        public static void AssertValid(
            this IModel model, string paramName, IDictionary<object, object> items = null, IServiceProvider serviceProvider = null)
        {
            var validationResults = model.Validate(items, serviceProvider);

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
    }
}
