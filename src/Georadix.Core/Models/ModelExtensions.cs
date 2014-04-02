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
        /// <param name="items">A dictionary of key/value pairs to make available data annotation attributes. This
        /// parameter is optional.</param>
        /// <param name="serviceProvider">The service provider to allow data annotation attributes to resolve
        /// additional dependencies. This parameter is optional.</param>
        /// <exception cref="ArgumentException">Contains the error message of the first 
        /// <see cref="ValidationResult"/>.</exception>
        public static void AssertValid(
            this Model model, IDictionary<object, object> items = null, IServiceProvider serviceProvider = null)
        {
            var validationResult = model.Validate(items, serviceProvider).FirstOrDefault();

            if (validationResult != null)
            {
                var errorMessage = string.Empty;

                if (validationResult.MemberNames.Any())
                {
                    errorMessage = string.Format("Properties: {0}. ", string.Join(", ", validationResult.MemberNames));
                }

                throw new ArgumentException(errorMessage + validationResult.ErrorMessage);
            }
        }
    }
}
