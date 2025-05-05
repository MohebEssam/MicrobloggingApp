using MicrobloggingApp.Infrastructure.Interfacses;

namespace MicrobloggingApp.API.Extensions
{
    public class ExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IExceptionLogger _exceptionLogger;

        public ExceptionLoggingMiddleware(RequestDelegate next, IExceptionLogger exceptionLogger)
        {
            _next = next;
            _exceptionLogger = exceptionLogger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _exceptionLogger.Log(ex, $"Unhandled exception at path: {context.Request.Path}");
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("An unexpected error occurred.");
            }
        }
    }

}
