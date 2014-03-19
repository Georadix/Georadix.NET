namespace Georadix.Data.Tests
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Xunit;

    public class SequentialGuidFixture
    {
        [Fact]
        public void NewGuidReturnsNonEmptyGuid()
        {
            var id = SequentialGuid.NewGuid();

            Assert.NotEqual(Guid.Empty, id);
        }

        [Fact]
        public void NewGuidsAreSequential()
        {
            Guid previous = SequentialGuid.NewGuid();
            Guid current = SequentialGuid.NewGuid();

            for (var i = 0; i < 1000; i++)
            {
                Assert.True(this.ParseSequentialBytes(current) >= this.ParseSequentialBytes(previous));

                previous = current;
                current = SequentialGuid.NewGuid();
            }
        }

        private long ParseSequentialBytes(Guid guid)
        {
            return long.Parse(
                BitConverter.ToString(guid.ToByteArray().Skip(10).Take(6).ToArray()).Replace("-", string.Empty),
                NumberStyles.HexNumber);
        }
    }
}