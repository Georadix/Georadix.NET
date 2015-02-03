namespace Georadix.Core.Data
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using Xunit;

    public class EntityUniqueConstraintExceptionFixture
    {
        private const string ErrorMessage = "Error message.";
        private static readonly Exception InnerException;
        private static readonly Guid Value = Guid.NewGuid();

        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations",
            Justification = "This is simply for making testing raised exceptions easier.")]
        static EntityUniqueConstraintExceptionFixture()
        {
            InnerException = new Exception("Inner error message.");
        }

        [Fact]
        public void CreateSucceeds()
        {
            var ex = new EntityUniqueConstraintException();

            Assert.NotEmpty(ex.Message);
            Assert.Null(ex.InnerException);
            Assert.Null(ex.Value);
        }

        [Fact]
        public void CreateWithMessageSucceeds()
        {
            var ex = new EntityUniqueConstraintException(ErrorMessage);

            Assert.Equal(ErrorMessage, ex.Message);
            Assert.Null(ex.InnerException);
            Assert.Null(ex.Value);
        }

        [Fact]
        public void CreateWithMessageAndInnerExceptionSucceeds()
        {
            var ex = new EntityUniqueConstraintException(ErrorMessage, InnerException);

            Assert.Equal(ErrorMessage, ex.Message);
            Assert.Equal(InnerException, ex.InnerException);
            Assert.Null(ex.Value);
        }

        [Fact]
        public void CreateWithMessageAndValueSucceeds()
        {
            var ex = new EntityUniqueConstraintException(ErrorMessage, Value);

            Assert.Contains(ErrorMessage, ex.Message);
            Assert.Contains(Value.ToString(), ex.Message);
            Assert.Null(ex.InnerException);
            Assert.Equal(Value, ex.Value);
        }

        [Fact]
        public void CreateWithMessageAndValueAndInnerExceptionSucceeds()
        {
            var ex = new EntityUniqueConstraintException(ErrorMessage, Value, InnerException);

            Assert.Contains(ErrorMessage, ex.Message);
            Assert.Contains(Value.ToString(), ex.Message);
            Assert.Equal(InnerException, ex.InnerException);
            Assert.Equal(Value, ex.Value);
        }

        [Fact]
        public void SerializeAndDeserializeSucceeds()
        {
            var ex = new EntityUniqueConstraintException(ErrorMessage, Value, InnerException);

            var stream = new MemoryStream();
            var formatter = new BinaryFormatter();

            formatter.Serialize(stream, ex);
            stream.Position = 0;

            var result = (EntityUniqueConstraintException)formatter.Deserialize(stream);

            Assert.Equal(ex.Message, result.Message);
            Assert.Equal(ex.InnerException.Message, result.InnerException.Message);
            Assert.Equal(ex.Value, result.Value);
        }
    }
}