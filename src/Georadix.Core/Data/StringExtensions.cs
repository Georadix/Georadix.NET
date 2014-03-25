namespace Georadix.Core.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Extensions method for <see cref="string"/>.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Tries to parse the string and returns a list of <see cref="OrderByExpression"/>.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="expressions">The expressions.</param>
        /// <returns>
        /// <see langword="true"/> if the source was parsed into a list of <see cref="OrderByExpression"/>,
        /// <see langword="false"/> otherwise.
        /// </returns>
        public static bool TryParseOrderByExpressions(this string source, out OrderByExpression[] expressions)
        {
            expressions = new OrderByExpression[] { };

            if (!string.IsNullOrWhiteSpace(source))
            {
                var orderByPattern = @"[a-z0-9]+(\s+(asc|desc))?";
                var pattern = string.Format(@"^\s*{0}\s*(,\s*{0}\s*)*$", orderByPattern);

                if (!Regex.IsMatch(source, pattern, RegexOptions.IgnoreCase))
                {
                    return false;
                }

                var expressionList = new List<OrderByExpression>();

                foreach (var expressionToken in Regex.Replace(source, @"\s+", " ").Split(','))
                {
                    var expressionParts = expressionToken.Trim().Split(' ');
                    var sortDirection = SortDirection.Ascending;

                    if (expressionParts.Length == 2 &&
                        expressionParts[1].Equals("desc", StringComparison.CurrentCultureIgnoreCase))
                    {
                        sortDirection = SortDirection.Descending;
                    }

                    expressionList.Add(new OrderByExpression(expressionParts[0], sortDirection));
                }

                expressions = expressionList.ToArray();
            }

            return true;
        }
    }
}