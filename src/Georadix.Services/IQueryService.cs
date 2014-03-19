namespace Georadix.Services
{
    using Georadix.Data;

    /// <summary>
    /// Defines a service used to execute queries.
    /// </summary>
    public interface IQueryService
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