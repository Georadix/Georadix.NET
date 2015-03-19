namespace Georadix.WebApi.Filters
{
    using log4net;
    using System;
    using System.Web.Http;
    using System.Web.Http.Filters;

    /// <summary>
    /// Represents a filter attribute that logs unhandled exceptions in the execution.
    /// </summary>
    public class LogExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly Func<Type, ILog> loggerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogExceptionFilterAttribute"/> class with a specified factory.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="loggerFactory"/> is <see langword="null"/>.
        /// </exception>
        public LogExceptionFilterAttribute(Func<Type, ILog> loggerFactory)
        {
            loggerFactory.AssertNotNull("loggerFactory");

            this.loggerFactory = loggerFactory;
        }

        /// <summary>
        /// Raises the exception event.
        /// </summary>
        /// <param name="actionExecutedContext">The context for the action.</param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var baseException = actionExecutedContext.Exception.GetBaseException();

            if (!(baseException is HttpResponseException))
            {
                var logger = this.loggerFactory(
                    actionExecutedContext.ActionContext.ControllerContext.Controller.GetType());

                logger.Error(baseException.Message, baseException);
            }
        }
    }
}