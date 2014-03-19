namespace Georadix.Services
{
    /// <summary>
    /// Defines a service used to execute commands.
    /// </summary>
    public interface ICommandService
    {
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command.</param>
        void Execute<TCommand>(TCommand command);
    }
}