using System.Diagnostics;

using Microsoft.Extensions.Primitives;

namespace WebApp;

public class CorrelationIdMiddleware: IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        context.Request.Headers.TryGetValue("x-correlation-id", out StringValues correlationIds);
        var correlationId = correlationIds.FirstOrDefault() ?? Guid.NewGuid().ToString();

        context.Items["CorrelationId"] = correlationId;
        context.Response.Headers["x-correlation-id"] = correlationId;

        using var scope = context.RequestServices.GetRequiredService<ILogger<CorrelationIdMiddleware>>()
            .BeginScope(new { CorrelationId = correlationId });
        await next(context);
    }
}