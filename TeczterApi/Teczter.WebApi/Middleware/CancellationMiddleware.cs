namespace Teczter.WebApi.Middleware;

internal class CancellationMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        if (context.RequestAborted.IsCancellationRequested)
        {
            context.Response.StatusCode = 499;
            await context.Response.WriteAsync("Request cancelled by user.");
            return;
        }

        try
        {
            await _next(context);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            context.Response.StatusCode = 499;

            if (!context.Response.HasStarted)
            {
                await context.Response.WriteAsync("Request cancelled by user during processing.");
            }
        }
    }
}
