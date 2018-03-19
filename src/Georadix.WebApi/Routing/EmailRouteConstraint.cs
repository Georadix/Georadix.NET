namespace Georadix.WebApi.Routing
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Net.Http;
    using System.Web.Http.Routing;

    /// <summary>
    /// Represents an email route constraint.
    /// </summary>
    public class EmailRouteConstraint : IHttpRouteConstraint
    {
        /// <summary>
        /// Determines whether this instance equals a specified route.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="route">The route to compare.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="values">A list of parameter values.</param>
        /// <param name="routeDirection">The route direction.</param>
        /// <returns>
        /// <c>True</c> if this instance equals a specified route; otherwise, <c>false</c>.
        /// </returns>
        public bool Match(
            HttpRequestMessage request,
            IHttpRoute route,
            string parameterName,
            IDictionary<string, object> values,
            HttpRouteDirection routeDirection)
        {
            if (values.TryGetValue(parameterName, out object value) && value != null)
            {
                var valueString = Convert.ToString(value, CultureInfo.InvariantCulture);

                var emailAttribute = new EmailAddressAttribute();

                return emailAttribute.IsValid(valueString);
            }

            return false;
        }
    }
}
