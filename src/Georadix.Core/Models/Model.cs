namespace Georadix.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a base model class used to transfer data.
    /// </summary>
    public abstract class Model
    {
        /// <summary>
        /// Validates this instance using the data annotations validator.
        /// </summary>
        /// <param name="items">A dictionary of key/value pairs to make available data annotation attributes. This
        /// parameter is optional.</param>
        /// <param name="serviceProvider">The service provider to allow data annotation attributes to resolve
        /// additional dependencies. This parameter is optional.</param>
        /// <returns>A list of <see cref="ValidationResult"/> containing validation errors. The list will be empty
        /// if the model is valid.</returns>
        public IEnumerable<ValidationResult> Validate(
            IDictionary<object, object> items = null, IServiceProvider serviceProvider = null)
        {
            var context = new ValidationContext(this, serviceProvider, items);

            var validationResults = new List<ValidationResult>();

            Validator.TryValidateObject(this, context, validationResults);

            return validationResults;
        }
    }
}
