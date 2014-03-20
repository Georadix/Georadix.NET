namespace Georadix.Cqs
{
    /// <summary>
    /// Defines a processor used to execute commands.
    /// </summary>
    public interface ICommandProcessor
    {
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command.</param>
        void Execute<TCommand>(TCommand command);
    }
}