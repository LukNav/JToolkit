using JToolkit.Middleware.Handlers.Models;

namespace JToolkit.Middleware.Wrappers.Models
{
    /// <inheritdoc />
    /// <summary>Exception type for Bad Request error</summary>
    public class BadRequestException : ErrorException
    {
        /// <inheritdoc />
        public override int StatusCode => 400;

        /// <inheritdoc />
        public BadRequestException(
            string reason = "validationFailed",
            string message = "The request is invalid.",
            Exception? innerException = null,
            IDictionary<string, object>? parameters = null)
            : base(reason, message, innerException, parameters)
        {
        }
    }
}