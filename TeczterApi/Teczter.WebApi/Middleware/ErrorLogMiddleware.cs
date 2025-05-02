using Teczter.Adapters.MiddlewareRepositories.ErrorLogRepository;
using Teczter.Data.MiddlewareModels;

namespace Teczter.WebApi.Middleware;

internal class ErrorLogMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
{
    private readonly RequestDelegate _next = next;
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception ex)
        {
            context.Items.TryGetValue("Request", out var request);

            var errorLog = new ErrorLog
            {
                User = context.User.Identity?.Name,
                ExceptionType = ex.GetType().Name,
                StackTrace = ex.StackTrace,
                Message = ex.Message,
                InnerExceptionMessage = ex.InnerException?.Message,
                RequestLog = request is RequestLog r ? r : null
            };

            try
            {
                using var scope = _scopeFactory.CreateScope();
                var errorLogger = scope.ServiceProvider.GetRequiredService<IErrorLogRepository>();
                await errorLogger.LogError(errorLog);
            }
            catch(Exception logEx)
            {
                Console.WriteLine($"ERROR LOGGING FAILED: {logEx.Message}");
            }

            throw;
        }
    }
}
