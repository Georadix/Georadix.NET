namespace Georadix.Core.Data
{
    using System;
    using System.Runtime.Serialization;
    using System.Text;

    /// <summary>
    /// The exception that is thrown when an entity cannot be found.
    /// </summary>
    [Serializable]
    public class EntityNotFoundException : Exception
    {
        private readonly Guid id;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
        /// </summary>
        public EntityNotFoundException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with a specified error
        /// message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public EntityNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with a specified error
        /// message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception
        /// is specified.
        /// </param>
        public EntityNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with a specified error
        /// message and the ID of the entity that was not found.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="id">The ID of the entity that was not found.</param>
        public EntityNotFoundException(string message, Guid id)
            : this(message)
        {
            this.id = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with a specified error
        /// message, the ID of the entity that was not found, and a reference to the inner exception that is the cause
        /// of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="id">The ID of the entity that was not found.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception
        /// is specified.
        /// </param>
        public EntityNotFoundException(string message, Guid id, Exception innerException)
            : this(message, innerException)
        {
            this.id = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with serialized data.
        /// </summary>
        /// <param name="info">
        /// The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="StreamingContext"/> that contains contextual information about the source or destination.
        /// </param>
        protected EntityNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.id = (Guid)info.GetValue("Id", typeof(Guid));
        }

        /// <summary>
        /// Gets the ID of the entity that was not found.
        /// </summary>
        public Guid Id
        {
            get { return this.id; }
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        public override string Message
        {
            get
            {
                return (this.Id == Guid.Empty) ? base.Message :
                    new StringBuilder(base.Message).AppendLine().Append(string.Format("ID: {0}", this.Id)).ToString();
            }
        }

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="SerializationInfo"/> with information about the
        /// exception.
        /// </summary>
        /// <param name="info">
        /// The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="StreamingContext"/> that contains contextual information about the source or destination.
        /// </param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("Id", this.id);
        }
    }
}