using JToolkit.Middleware.Models;

namespace JToolkit.Middleware.Exceptions
{
    /// <inheritdoc />
    /// <summary>Exception type for Internal Server error</summary>
    public class InternalServerErrorException : ErrorException
    {
        /// <inheritdoc />
        public override int StatusCode => 500;

        /// <inheritdoc />
        public InternalServerErrorException(
            string reason = "internalServerError",
            string message = "Unexpected internal server error.",
            Exception? innerException = null,
            IDictionary<string, object>? parameters = null)
            : base(reason, message, innerException, parameters)
        {
        }
    }
}