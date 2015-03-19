namespace Georadix.WebApi.Filters
{
    using Georadix.WebApi.Resources;
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    /// <summary>
    /// Represents a filter attribute used to check if the arguments are null.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class CheckArgumentsForNullAttribute : ActionFilterAttribute
    {
        private readonly string[] arguments;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckArgumentsForNullAttribute" /> class with specified
        /// arguments.
        /// </summary>
        /// <param name="arguments">
        /// The arguments to check for <see langword="null"/>, or none to include all of them.
        /// </param>
        public CheckArgumentsForNullAttribute(params string[] arguments)
        {
            this.arguments = arguments;
        }

        /// <summary>
        /// Occurs before the action method is invoked.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var args = (this.arguments == null) || (this.arguments.Length == 0) ?
                actionContext.ActionArguments :
                actionContext.ActionArguments.Where(arg => this.arguments.Contains(arg.Key));

            args.Where(arg => arg.Value == null).ForEach(
                arg => actionContext.ModelState.AddModelError(arg.Key, InvariantStrings.ParameterIsRequired));

            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest, actionContext.ModelState);
            }
        }
    }
}