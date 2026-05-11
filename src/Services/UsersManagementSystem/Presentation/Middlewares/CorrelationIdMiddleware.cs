
namespace Presentation.Middlewares
{
    public class CorrelationIdMiddleware
    {
        private const string CorrelationIdHeader = "X-Correlation-Id";
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Guid correlationId;

            // try retrieve correlation ID from the incoming request header, if not present generate a new one
            if (context.Request.Headers.TryGetValue(CorrelationIdHeader, out var correlationIdValue) && Guid.TryParse(correlationIdValue, out var parsedCorrelationId))
                correlationId = parsedCorrelationId;
            else
                correlationId = Guid.NewGuid();

            // Store the correlation ID in the HttpContext.Items for later retrieval in the request pipeline
            context.Items["CorrelationId"] = correlationId;


            // Return it to caller
            context.Response.OnStarting(() =>
            {
                context.Response.Headers[CorrelationIdHeader] = correlationId.ToString();
                return Task.CompletedTask;
            });

            await _next(context);

        }


    }
}
