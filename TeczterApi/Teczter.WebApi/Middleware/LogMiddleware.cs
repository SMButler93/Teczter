using Teczter.Domain.Exceptions;

namespace Teczter.WebApi.Middleware;

internal class LogMiddleware(RequestDelegate next, ILogger<LogMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<LogMiddleware> _logger = logger;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (TeczterValidationException ex)
        {
            try
            {
                _logger.LogWarning(ex, $"A validation error was thrown: {ex.Message}");
            }
            catch (Exception logEx)
            {
                Console.WriteLine($"ERROR LOGGING FAILED: {logEx.Message}");
            }

            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new
                {
                    Error = ex.Message
                });
            }
        }
        catch (Exception ex)
        {
            try
            {
                _logger.LogError(ex, $"An unexpected error occurred: {ex.Message}");
            }
            catch (Exception logEx)
            {
                Console.WriteLine($"ERROR LOGGING FAILED: {logEx.Message}");
            }

            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new
                {
                    Error = ex.Message
                });
            }
        }
    }
}
