namespace System.Security.Claims
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines methods and properties that extend security claim types.
    /// </summary>
    public static class ClaimsExtensions
    {
        /// <summary>
        /// Determines whether the principal contains the specified claims.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="claims">The claims.</param>
        /// <returns><c>true</c> if the principal contains the specified claims; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="claims"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">
        /// One of the items in the <paramref name="claims"/> collection is <see langword="null"/>.
        /// </exception>
        public static bool HasClaims(this ClaimsPrincipal principal, IEnumerable<Claim> claims)
        {
            claims.AssertNotNull(true, "claims");

            foreach (var claim in claims)
            {
                if (!principal.HasClaim(claim.Type, claim.Value))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether the principal contains the specified claim types.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="claimTypes">The claim types.</param>
        /// <returns>
        /// <c>true</c> if the principal contains the specified claim types; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="claimTypes"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// One of the items in the <paramref name="claimTypes"/> collection is <see langword="null"/>.
        /// </exception>
        public static bool HasClaimTypes(this ClaimsPrincipal principal, IEnumerable<string> claimTypes)
        {
            claimTypes.AssertNotNull(true, "claimTypes");

            foreach (var type in claimTypes)
            {
                if (!principal.HasClaim(c => c.Type == type))
                {
                    return false;
                }
            }

            return true;
        }
    }
}