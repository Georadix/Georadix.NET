namespace Georadix.Core.Security
{
    using System;
    using System.Runtime.Serialization;
    using System.Security.Cryptography;

    /// <summary>
    /// Represents an RFC 2898 encrypted value.
    /// </summary>
    [Serializable]
    public class Rfc2898EncryptedValue : ISerializable
    {
        /// <summary>
        /// The default number of bytes to generate for a hash.
        /// </summary>
        public const int DefaultHashSize = 20;

        /// <summary>
        /// The default number of iterations for the hashing operation.
        /// </summary>
        public const int DefaultIterations = 10000;

        /// <summary>
        /// The default number of bytes to generate for a salt.
        /// </summary>
        public const int DefaultSaltSize = 16;

        private static Random randomSource = new Random();
        private readonly byte[] hash;
        private readonly int iterations;
        private readonly byte[] salt;

        /// <summary>
        /// Initializes a new instance of the <see cref="Rfc2898EncryptedValue"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="hashSize">The number of bytes to generate for the hash.</param>
        /// <param name="iterations">The number of iterations for the hashing operation.</param>
        /// <param name="saltSize">The number of bytes to generate for the salt.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> is empty.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="hashSize"/> or <paramref name="iterations"/> is less than 1 or
        /// <paramref name="saltSize"/> is less than 8.
        /// </exception>
        public Rfc2898EncryptedValue(
            byte[] value,
            int hashSize = DefaultHashSize,
            int iterations = DefaultIterations,
            int saltSize = DefaultSaltSize)
            : this(value, GenerateSalt(saltSize), hashSize, iterations)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rfc2898EncryptedValue"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="salt">The salt to use for hashing the value.</param>
        /// <param name="hashSize">The number of bytes to generate for the hash.</param>
        /// <param name="iterations">The number of iterations for the hashing operation.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> or <paramref name="salt"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> or <paramref name="salt"/> is empty.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="hashSize"/> or <paramref name="iterations"/> is less than 1.
        /// </exception>
        public Rfc2898EncryptedValue(
            byte[] value,
            byte[] salt,
            int hashSize = DefaultHashSize,
            int iterations = DefaultIterations)
        {
            value.AssertNotNullOrEmpty(false, "value");
            salt.AssertNotNullOrEmpty(false, "salt");
            hashSize.AssertGreaterThan(0, "hashSize");
            iterations.AssertGreaterThan(0, "iterations");

            this.salt = salt;
            this.hash = GenerateHash(value, this.salt, hashSize, iterations);
            this.iterations = iterations;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rfc2898EncryptedValue"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor should only be used to deserialize an instance.
        /// </remarks>
        /// <param name="info">The <see cref="T:SerializationInfo" /> to populate with data.</param>
        /// <param name="context">
        /// The destination (see <see cref="T:StreamingContext" />) for this serialization.
        /// </param>
        protected Rfc2898EncryptedValue(SerializationInfo info, StreamingContext context)
        {
            this.hash = (byte[])info.GetValue("Hash", typeof(byte[]));
            this.iterations = (int)info.GetValue("Iterations", typeof(int));
            this.salt = (byte[])info.GetValue("Salt", typeof(byte[]));
        }

        /// <summary>
        /// Gets the hash.
        /// </summary>
        public byte[] Hash
        {
            get { return this.hash; }
        }

        /// <summary>
        /// Gets the iterations.
        /// </summary>
        public int Iterations
        {
            get { return this.iterations; }
        }

        /// <summary>
        /// Gets the salt.
        /// </summary>
        public byte[] Salt
        {
            get { return this.salt; }
        }

        /// <summary>
        /// Populates a <see cref="T:SerializationInfo"/> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="T:SerializationInfo" /> to populate with data.</param>
        /// <param name="context">The destination (see <see cref="T:StreamingContext" />) for this serialization.</param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Hash", this.Hash, typeof(byte[]));
            info.AddValue("Iterations", this.Iterations);
            info.AddValue("Salt", this.Salt, typeof(byte[]));
        }

        private static byte[] GenerateHash(byte[] value, byte[] salt, int hashSize, int iterations)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(value, salt, iterations))
            {
                return pbkdf2.GetBytes(hashSize);
            }
        }

        private static byte[] GenerateSalt(int saltSize)
        {
            saltSize.AssertGreaterThanOrEqualTo(8, "saltSize");

            var bytes = new byte[saltSize];

            lock (randomSource)
            {
                randomSource.NextBytes(bytes);
            }

            return bytes;
        }
    }
}