namespace Georadix.Cqs
{
    using System;

    /// <summary>
    /// Represents a processor used to execute commands.
    /// </summary>
    public class CommandProcessor : ICommandProcessor
    {
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandProcessor"/> class with a specified service provider.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serviceProvider"/> is <see langword="null"/>.
        /// </exception>
        public CommandProcessor(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }

            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command.</param>
        /// <exception cref="ArgumentNullException"><paramref name="command"/> is <see langword="null"/>.</exception>
        public void Execute<TCommand>(TCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            var commandType = command.GetType();
            var commandHandlerType = typeof(ICommandHandler<>).MakeGenericType(commandType);

            dynamic commandHandler = this.serviceProvider.GetService(commandHandlerType);

            commandHandler.Handle((dynamic)command);
        }
    }
}