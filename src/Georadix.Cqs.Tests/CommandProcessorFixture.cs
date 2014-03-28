namespace Georadix.Cqs
{
    using Moq;
    using System;
    using Xunit;

    public class CommandProcessorFixture
    {
        [Fact]
        public void ConstructorWithNullServiceProviderThrowsArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new CommandProcessor(null));

            Assert.Equal("serviceProvider", ex.ParamName);
        }

        [Fact]
        public void ExecuteWithNullCommandThrowsArgumentNullException()
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);

            var sut = new CommandProcessor(serviceProviderMock.Object);

            TestCommand command = null;

            var ex = Assert.Throws<ArgumentNullException>(() => sut.Execute(command));

            Assert.Equal("command", ex.ParamName);
        }

        [Fact]
        public void ExecuteWithValidCommandExecutesCommand()
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);
            var commandHandlerMock = new Mock<ICommandHandler<TestCommand>>(MockBehavior.Strict);

            var sut = new CommandProcessor(serviceProviderMock.Object);

            var command = new TestCommand();

            serviceProviderMock
                .Setup(sp => sp.GetService(typeof(ICommandHandler<TestCommand>)))
                .Returns(commandHandlerMock.Object);

            commandHandlerMock.Setup(ch => ch.Handle(command));

            sut.Execute(command);

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