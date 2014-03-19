namespace Georadix.Services.Tests
{
    using Georadix.Data;
    using Moq;
    using System;
    using Xunit;

    public class CommandServiceFixture
    {
        [Fact]
        public void CreateWithNullServiceProviderThrowsArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new CommandService(null));

            Assert.Equal("serviceProvider", ex.ParamName);
        }

        [Fact]
        public void ExecuteWithNullCommandThrowsArgumentNullException()
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);

            var service = new CommandService(serviceProviderMock.Object);

            TestCommand command = null;

            var ex = Assert.Throws<ArgumentNullException>(() => service.Execute(command));

            Assert.Equal("command", ex.ParamName);
        }

        [Fact]
        public void ExecuteWithValidCommandExecutesCommand()
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);
            var commandHandlerMock = new Mock<ICommandHandler<TestCommand>>(MockBehavior.Strict);

            var service = new CommandService(serviceProviderMock.Object);

            var command = new TestCommand();

            serviceProviderMock
                .Setup(sp => sp.GetService(typeof(ICommandHandler<TestCommand>)))
                .Returns(commandHandlerMock.Object);

            commandHandlerMock.Setup(ch => ch.Handle(command));

            service.Execute(command);

            serviceProviderMock.VerifyAll();
            commandHandlerMock.VerifyAll();
        }

        public class TestCommand : Command
        {
            public TestCommand()
            {
            }
        }
    }
}