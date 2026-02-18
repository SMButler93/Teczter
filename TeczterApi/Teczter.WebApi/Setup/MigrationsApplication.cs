using Microsoft.EntityFrameworkCore;
using Teczter.Data;
using Teczter.Infrastructure.Auth;

namespace Teczter.WebApi.Setup;

public static class MigrationsApplication
{
    public static async Task ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        var appDb = services.GetRequiredService<TeczterDbContext>();
        var userDb = services.GetRequiredService<UserDbContext>();

        await appDb.Database.MigrateAsync();
        await userDb.Database.MigrateAsync();
    }
}