namespace Georadix.Core.Data
{
    using System;

    /// <summary>
    /// Represents an expression to order results by a property in the specified <see cref="SortDirection"/>.
    /// </summary>
    public class OrderByExpression
    {
        private readonly string propertyName;
        private readonly SortDirection direction;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderByExpression"/> class.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="direction">The direction.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="propertyName"/> is <see langword="null"/>.
        /// </exception>
        public OrderByExpression(string propertyName, SortDirection direction)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }

            this.propertyName = propertyName;
            this.direction = direction;
        }

        /// <summary>
        /// Gets the sort direction.
        /// </summary>
        /// <value>
        /// The direction.
        /// </value>
        public SortDirection Direction
        {
            get { return this.direction; }
        }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string PropertyName
        {
            get { return this.propertyName; }
        }
    }
}