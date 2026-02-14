namespace Teczter.WebApi.Middleware;

internal static class MiddlewareRegistration
{
    public static void UseTeczterMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<LogMiddleware>();
    }
}
