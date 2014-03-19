namespace Georadix.Data
{
    using System;

    /// <summary>
    /// Defines a command.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Gets the ID.
        /// </summary>
        /// <value>The ID.</value>
        Guid Id { get; }
    }
}
