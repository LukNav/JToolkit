namespace JToolkit.Middleware.Models
{
    /// <inheritdoc />
    /// <summary>Exception type for Forbidden error</summary>
    public class ForbiddenException : ErrorException
    {
        /// <inheritdoc />
        public override int StatusCode => 403;

        /// <inheritdoc />
        public ForbiddenException(
            string reason = "authorizationFailed",
            string message = "The subject of the token does not have the permissions that are required to access the resource.",
            Exception? innerException = null,
            IDictionary<string, object>? parameters = null)
            : base(reason, message, innerException, parameters)
        {
        }
    }
}