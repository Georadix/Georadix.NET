namespace Georadix.Services
{
    using Georadix.Data;
    using System;

    /// <summary>
    /// Represents a service used to execute queries.
    /// </summary>
    public class QueryService : IQueryService
    {
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryService"/> class with a specified service provider.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serviceProvider"/> is <see langword="null"/>.
        /// </exception>
        public QueryService(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }

            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Executes the specified query.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns>The query result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="query"/> is <see langword="null"/>.</exception>
        public TResult Execute<TResult>(IQuery<TResult> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var queryType = query.GetType();
            var queryHandlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, typeof(TResult));

            dynamic queryHandler = this.serviceProvider.GetService(queryHandlerType);

            return queryHandler.Handle((dynamic)query);
        }
    }
}
