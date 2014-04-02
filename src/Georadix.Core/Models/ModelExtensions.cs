namespace System
{
    using Georadix.Core.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    /// <summary>
    /// Extensions to the <see cref="Model"/> class.
    /// </summary>
    /// <remarks>
    /// Those are included in the system namespace for convenience.
    /// </remarks>
    public static class ModelExtensions
    {
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
            this Model model, string paramName, IDictionary<object, object> items = null, IServiceProvider serviceProvider = null)
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
