namespace Georadix.WebApi.Filters
{
    using log4net;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Filters;

    /// <summary>
    /// Logs exception and returns a standard HTTP 500 status code.
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
            if (loggerFactory == null)
            {
                throw new ArgumentNullException("loggerFactory");
            }

            this.loggerFactory = loggerFactory;
        }

        /// <summary>
        /// Raises the exception event.
        /// </summary>
        /// <param name="actionExecutedContext">The context for the action.</param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var logger = this.loggerFactory(
                actionExecutedContext.ActionContext.ControllerContext.Controller.GetType());

            var baseException = actionExecutedContext.Exception.GetBaseException();

            logger.Error(baseException.Message, baseException);

            actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            actionExecutedContext.Response.Content =
                new StringContent("An error occurred while processing your request.");
        }
    }
}