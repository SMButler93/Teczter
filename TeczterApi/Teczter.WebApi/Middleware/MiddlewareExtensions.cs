namespace Teczter.WebApi.Middleware;

internal static class MiddlewareExtensions
{
    public static void UseTeczterMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<LogMiddleware>();
    }
}
