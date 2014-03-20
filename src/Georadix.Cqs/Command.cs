namespace Georadix.Cqs
{
    using Georadix.Core;
    using System;

    /// <summary>
    /// Abstract base class for all commands.
    /// </summary>
    public abstract class Command : ICommand
    {
        private readonly Guid id;

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        public Command()
        {
            this.id = SequentialGuid.NewGuid();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class with a specified ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        protected Command(Guid id)
        {
            this.id = id;
        }

        /// <summary>
        /// Gets the ID.
        /// </summary>
        /// <value>
        /// The ID.
        /// </value>
        public Guid Id
        {
            get { return this.id; }
        }
    }
}