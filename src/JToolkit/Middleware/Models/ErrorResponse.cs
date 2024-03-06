using System.ComponentModel.DataAnnotations;

namespace JToolkit.Middleware.Models
{
    public class ErrorResponse
    {
        /// <summary>
        /// Unique, alphanumeric string, that represents human readable purpose of the numerical symbol
        /// </summary>
        /// <example>notFound</example>
        [Required]
        public string? Reason { get; set; }

        /// <summary>Human readable description of error</summary>
        /// <example>Resource not found.</example>
        [Required]
        public string Message { get; set; }

        public IDictionary<string, object>? Params { get; set; }

        public ErrorResponse(string? reason, string message, IDictionary<string, object>? @params = null)
        {
            Reason = reason;
            Message = message;
            if (@params == null)
                return;
            Params = new Dictionary<string, object>(@params);
        }
    }
}