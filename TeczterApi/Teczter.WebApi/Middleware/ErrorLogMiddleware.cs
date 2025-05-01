using Microsoft.IdentityModel.Tokens;
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
            var errorLog = new ErrorLog
            {
                User = context.User.Identity?.Name,
                ExceptionType = ex.GetType().Name,
                StackTrace = ex.StackTrace,
                Message = ex.Message,
                InnerExceptionMessage = ex.InnerException?.Message,
                Path = context.Request.Path,
                Method = context.Request.Method,
                Query = context.Request.QueryString.ToString().IsNullOrEmpty() ? null : context.Request.QueryString.ToString(),
                StatusCode = context.Response.StatusCode.ToString()
            };

            try
            {
                using var scope = _scopeFactory.CreateScope();
                var errorLogger = scope.ServiceProvider.GetRequiredService<IErrorLogRepository>();
                await errorLogger.LogError(errorLog);
            }
            catch(Exception logEx)
            {
                Console.WriteLine($"ERROR LOGGING: {logEx.Message}");
            }

            throw;
        }
    }
}
