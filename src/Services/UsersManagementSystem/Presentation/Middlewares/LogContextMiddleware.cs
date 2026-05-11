using Serilog.Context;

namespace Presentation.Middlewares
{
    public class LogContextMiddleware
    {
        private readonly RequestDelegate _next;
        public LogContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = context.Items["CorrelationId"]?.ToString();

            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                await _next(context);
            }

        }
    }
}
