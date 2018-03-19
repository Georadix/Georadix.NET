namespace Georadix.Cqs
{
    /// <summary>
    /// Defines a generic command handler.
    /// </summary>
    /// <typeparam name="TCommand">The type of command.</typeparam>
    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        void Handle(TCommand command);
    }
}
