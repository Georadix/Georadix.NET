namespace Georadix.Cqs
{
    /// <summary>
    /// Defines a processor used to execute queries.
    /// </summary>
    public interface IQueryProcessor
    {
        /// <summary>
        /// Executes the specified query.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns>The query result.</returns>
        TResult Execute<TResult>(IQuery<TResult> query);
    }
}