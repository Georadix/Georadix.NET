namespace Georadix.Core.Data
{
    using System;
    using System.Runtime.Serialization;
    using System.Text;

    /// <summary>
    /// The exception that is thrown when creating an entity violates a unique constraint.
    /// </summary>
    [Serializable]
    public class EntityUniqueConstraintException : Exception
    {
        private readonly object value;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityUniqueConstraintException"/> class.
        /// </summary>
        public EntityUniqueConstraintException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityUniqueConstraintException"/> class with
        /// a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public EntityUniqueConstraintException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityUniqueConstraintException"/> class with a
        /// specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception
        /// is specified.
        /// </param>
        public EntityUniqueConstraintException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityUniqueConstraintException" /> class with a specified
        /// error message, the unique property and its value.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="value">The value.</param>
        public EntityUniqueConstraintException(string message, object value)
            : this(message)
        {
            this.value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityUniqueConstraintException" /> class with a specified
        /// error message, the unique property and its value, and a reference to the inner exception that is the
        /// cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="value">The value.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or <see langword="null" /> if no inner exception
        /// is specified.
        /// </param>
        public EntityUniqueConstraintException(
            string message, object value, Exception innerException)
            : this(message, innerException)
        {
            this.value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityUniqueConstraintException"/> class with serialized
        /// data.
        /// </summary>
        /// <param name="info">
        /// The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="StreamingContext"/> that contains contextual information about the source or destination.
        /// </param>
        protected EntityUniqueConstraintException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.value = info.GetValue("Value", typeof(object));
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        public override string Message
        {
            get
            {
                return this.Value == null ? base.Message :
                    new StringBuilder(base.Message).AppendLine().Append(
                    string.Format("Value: {0}", this.Value)).ToString();
            }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public object Value
        {
            get { return this.value; }
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

            info.AddValue("Value", this.value);
        }
    }
}