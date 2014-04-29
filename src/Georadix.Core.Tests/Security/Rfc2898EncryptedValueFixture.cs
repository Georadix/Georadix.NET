namespace Georadix.Core.Security
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using Xunit;

    public class Rfc2898EncryptedValueFixture
    {
        private byte[] sampleSalt = Encoding.UTF8.GetBytes("salt value");
        private byte[] sampleValue = Encoding.UTF8.GetBytes("value");

        [Fact]
        public void ConstructorsReturnsInitializedInstance()
        {
            var hashSize = 40;
            var iterations = 5000;
            var saltSize = 20;

            var sut = new Rfc2898EncryptedValue(this.sampleValue, hashSize, iterations, saltSize);

            Assert.Equal(hashSize, sut.Hash.Length);
            Assert.Equal(iterations, sut.Iterations);
            Assert.Equal(saltSize, sut.Salt.Length);

            sut = new Rfc2898EncryptedValue(this.sampleValue, this.sampleSalt, hashSize, iterations);

            Assert.Equal(hashSize, sut.Hash.Length);
            Assert.Equal(iterations, sut.Iterations);
            Assert.Same(this.sampleSalt, sut.Salt);
        }

        [Fact]
        public void ConstructorsWithDefaultsReturnsInitializedInstance()
        {
            var sut = new Rfc2898EncryptedValue(this.sampleValue);

            Assert.Equal(Rfc2898EncryptedValue.DefaultHashSize, sut.Hash.Length);
            Assert.Equal(Rfc2898EncryptedValue.DefaultIterations, sut.Iterations);
            Assert.Equal(Rfc2898EncryptedValue.DefaultSaltSize, sut.Salt.Length);

            sut = new Rfc2898EncryptedValue(this.sampleValue, this.sampleSalt);

            Assert.Equal(Rfc2898EncryptedValue.DefaultHashSize, sut.Hash.Length);
            Assert.Equal(Rfc2898EncryptedValue.DefaultIterations, sut.Iterations);
            Assert.Same(this.sampleSalt, sut.Salt);
        }

        [Fact]
        public void ConstructorsWithEmptyValueThrowsArgumentException()
        {
            var ex1 = Assert.Throws<ArgumentException>(() => new Rfc2898EncryptedValue(new byte[] { }));

            Assert.Equal("value", ex1.ParamName);

            var ex2 = Assert.Throws<ArgumentException>(
                () => new Rfc2898EncryptedValue(new byte[] { }, this.sampleSalt));

            Assert.Equal("value", ex2.ParamName);
        }

        [Fact]
        public void ConstructorsWithHashSizeLessThan1ThrowsArgumentOutOfRangeException()
        {
            var ex1 = Assert.Throws<ArgumentOutOfRangeException>(
                () => new Rfc2898EncryptedValue(this.sampleValue, hashSize: 0));

            Assert.Equal("hashSize", ex1.ParamName);

            var ex2 = Assert.Throws<ArgumentOutOfRangeException>(
                () => new Rfc2898EncryptedValue(this.sampleValue, this.sampleSalt, hashSize: 0));

            Assert.Equal("hashSize", ex2.ParamName);
        }

        [Fact]
        public void ConstructorsWithIterationsLessThan1ThrowsArgumentOutOfRangeException()
        {
            var ex1 = Assert.Throws<ArgumentOutOfRangeException>(
                () => new Rfc2898EncryptedValue(this.sampleValue, iterations: 0));

            Assert.Equal("iterations", ex1.ParamName);

            var ex2 = Assert.Throws<ArgumentOutOfRangeException>(
                () => new Rfc2898EncryptedValue(this.sampleValue, this.sampleSalt, iterations: 0));

            Assert.Equal("iterations", ex2.ParamName);
        }

        [Fact]
        public void ConstructorsWithNullValueThrowsArgumentNullException()
        {
            var ex1 = Assert.Throws<ArgumentNullException>(() => new Rfc2898EncryptedValue(null));

            Assert.Equal("value", ex1.ParamName);

            var ex2 = Assert.Throws<ArgumentNullException>(
                () => new Rfc2898EncryptedValue(null, this.sampleSalt));

            Assert.Equal("value", ex2.ParamName);
        }

        [Fact]
        public void ConstructorWithEmptySaltThrowsArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentException>(
                () => new Rfc2898EncryptedValue(this.sampleValue, new byte[] { }));

            Assert.Equal("salt", ex.ParamName);
        }

        [Fact]
        public void ConstructorWithNullSaltThrowsArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new Rfc2898EncryptedValue(this.sampleValue, null));

            Assert.Equal("salt", ex.ParamName);
        }

        [Fact]
        public void ConstructorWithSaltSizeLessThan8ThrowsArgumentOutOfRangeException()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => new Rfc2898EncryptedValue(this.sampleValue, saltSize: 7));

            Assert.Equal("saltSize", ex.ParamName);
        }

        [Fact]
        public void IsSerializableAndDeserializable()
        {
            var sut = new Rfc2898EncryptedValue(Encoding.UTF8.GetBytes("value"));
            Rfc2898EncryptedValue deserialized = null;

            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();

                formatter.Serialize(stream, sut);

                stream.Seek(0, SeekOrigin.Begin);

                deserialized = (Rfc2898EncryptedValue)formatter.Deserialize(stream);
            }

            Assert.Equal(sut.Hash, deserialized.Hash);
            Assert.Equal(sut.Iterations, deserialized.Iterations);
            Assert.Equal(sut.Salt, deserialized.Salt);
        }
    }
}