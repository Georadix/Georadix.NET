namespace Georadix.Web
{
    using System.IdentityModel.Tokens;

    /// <summary>
    /// Represents a JWT validation parameters provider.
    /// </summary>
    public interface IJwtValidationParametersProvider
    {
        /// <summary>
        /// Gets the token validation parameters.
        /// </summary>
        TokenValidationParameters TokenValidationParameters { get; }
    }
}