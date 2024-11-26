using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using SensorApis.Models;  // Import your models if needed for the error response

namespace SensorApis.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);  // Call the next middleware
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");

                // Customize your error response
                var errorResponse = new
                {
                    Message = "An unexpected error occurred. Please try again later.",
                    Details = ex.Message
                };

                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
}
