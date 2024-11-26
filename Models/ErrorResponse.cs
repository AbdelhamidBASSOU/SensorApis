using System;

namespace SensorApis.Models
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public string Detail { get; set; }
        public string StackTrace { get; set; }

        public ErrorResponse(string message, string detail, string stackTrace)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Detail = detail ?? throw new ArgumentNullException(nameof(detail));
            StackTrace = stackTrace ?? throw new ArgumentNullException(nameof(stackTrace));
        }
    }
}
