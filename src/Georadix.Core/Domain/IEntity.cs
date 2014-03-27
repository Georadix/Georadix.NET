namespace Georadix.Core.Domain
{
    using System;

    /// <summary>
    /// Defines a uniquely identifiable entity.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        Guid Id { get; }
    }
}