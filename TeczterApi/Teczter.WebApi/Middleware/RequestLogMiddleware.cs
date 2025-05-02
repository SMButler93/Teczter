using Teczter.Adapters.MiddlewareRepositories.RequestLogRepository;
using Teczter.Data.MiddlewareModels;

namespace Teczter.WebApi.Middleware;

internal class RequestLogMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
{
    private readonly RequestDelegate _next = next;
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    public async Task Invoke(HttpContext context)
    {
        await _next(context);

        var request = new RequestLog()
        {
            User = context.User.Identity?.Name,
            Path = context.Request.Path,
            Method = context.Request.Method,
            Query = context.Request.QueryString.ToString(),
            StatusCode = context.Response.StatusCode.ToString()
        };

        context.Items["Request"] = request;

        using var scope = _scopeFactory.CreateScope();
        var requestLogger = scope.ServiceProvider.GetRequiredService<IRequestLogRepository>();
        await requestLogger.LogRequest(request);
    }
}
