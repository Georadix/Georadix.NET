namespace Georadix.Services
{
    using Georadix.Data;
    using System;

    /// <summary>
    /// Represents a service used to execute commands.
    /// </summary>
    public class CommandService : ICommandService
    {
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandService"/> class with a specified service provider.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serviceProvider"/> is <see langword="null"/>.
        /// </exception>
        public CommandService(IServiceProvider serviceProvider)
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