namespace Georadix.Core.Data
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using Xunit;

    public class EntityNotFoundExceptionFixture
    {
        private const string ErrorMessage = "Error message.";
        private static readonly Guid Id = Guid.NewGuid();
        private static readonly Exception InnerException;

        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations",
            Justification = "This is simply for making testing raised exceptions easier.")]
        static EntityNotFoundExceptionFixture()
        {
            InnerException = new Exception("Inner error message.");
        }

        [Fact]
        public void CreateSucceeds()
        {
            var ex = new EntityNotFoundException();

            Assert.NotEmpty(ex.Message);
            Assert.Null(ex.InnerException);
            Assert.Equal(Guid.Empty, ex.Id);
        }

        [Fact]
        public void CreateWithMessageAndIdAndInnerExceptionSucceeds()
        {
            var ex = new EntityNotFoundException(ErrorMessage, Id, InnerException);

            Assert.Contains(ErrorMessage, ex.Message);
            Assert.Contains(Id.ToString(), ex.Message);
            Assert.Equal(InnerException, ex.InnerException);
            Assert.Equal(Id, ex.Id);
        }

        [Fact]
        public void CreateWithMessageAndIdSucceeds()
        {
            var ex = new EntityNotFoundException(ErrorMessage, Id);

            Assert.Contains(ErrorMessage, ex.Message);
            Assert.Contains(Id.ToString(), ex.Message);
            Assert.Null(ex.InnerException);
            Assert.Equal(Id, ex.Id);
        }

        [Fact]
        public void CreateWithMessageAndInnerExceptionSucceeds()
        {
            var ex = new EntityNotFoundException(ErrorMessage, InnerException);

            Assert.Equal(ErrorMessage, ex.Message);
            Assert.Equal(InnerException, ex.InnerException);
            Assert.Equal(Guid.Empty, ex.Id);
        }

        [Fact]
        public void CreateWithMessageSucceeds()
        {
            var ex = new EntityNotFoundException(ErrorMessage);

            Assert.Equal(ErrorMessage, ex.Message);
            Assert.Null(ex.InnerException);
            Assert.Equal(Guid.Empty, ex.Id);
        }

        [Fact]
        public void SerializeAndDeserializeSucceeds()
        {
            var ex = new EntityNotFoundException(ErrorMessage, Id, InnerException);

            var stream = new MemoryStream();
            var formatter = new BinaryFormatter();

            formatter.Serialize(stream, ex);
            stream.Position = 0;

            var result = (EntityNotFoundException)formatter.Deserialize(stream);

            Assert.Equal(ex.Message, result.Message);
            Assert.Equal(ex.InnerException.Message, result.InnerException.Message);
            Assert.Equal(ex.Id, result.Id);
        }
    }
}
