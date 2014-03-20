namespace Georadix.Cqs.Tests
{
    using Newtonsoft.Json;
    using System;
    using Xunit;

    public class CommandFixture
    {
        [Fact]
        public void CreateSetsId()
        {
            var command = new TestCommand();

            Assert.NotEqual(Guid.Empty, command.Id);
        }

        [Fact]
        public void IsSerializableAndDeserializableInJson()
        {
            var command1 = new TestCommand();

            string output = JsonConvert.SerializeObject(command1);

            Assert.NotNull(output);
            Assert.True(output.Length > 0);

            var command2 = JsonConvert.DeserializeObject<TestCommand>(output);

            Assert.NotNull(command2);
            Assert.Equal(command1.Id, command2.Id);
        }

        private class TestCommand : Command
        {
            public TestCommand()
            {
            }

            [JsonConstructor]
            private TestCommand(Guid id)
                : base(id)
            {
            }
        }
    }
}