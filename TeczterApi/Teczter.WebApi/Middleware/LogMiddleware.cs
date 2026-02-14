namespace Teczter.WebApi.Middleware;

internal class LogMiddleware(RequestDelegate _next, ILogger<LogMiddleware> _logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An unexpected error occurred: {ex.Message}");

            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new
                {
                    Error = "An unexpected error occurred"
                });
            }
        }
    }
}
