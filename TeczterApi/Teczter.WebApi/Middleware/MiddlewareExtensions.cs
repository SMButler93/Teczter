namespace Teczter.WebApi.Middleware;

internal static class MiddlewareExtensions
{
    public static void UseTeczterMiddleware(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<RequestLogMiddleware>();
        builder.UseMiddleware<ErrorLogMiddleware>();
    }
}
