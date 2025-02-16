using System.Diagnostics;

namespace RideSharingApp.Middleware
{
    public class LoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggerMiddleware> _logger;

        public LoggerMiddleware(RequestDelegate next, ILogger<LoggerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log request details
            _logger.LogInformation($"Incoming Request: {context.Request.Method} {context.Request.Path}");

            var stopwatch = Stopwatch.StartNew();
            await _next(context); // Call the next middleware
            stopwatch.Stop();

            // Log response details
            _logger.LogInformation($"Outgoing Response: {context.Response.StatusCode} - Time Taken: {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}
