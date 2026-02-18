using Teczter.Data;
using Teczter.Infrastructure.Auth;

namespace Teczter.WebApi.DemoModeConfigAndSetup;

internal static class DemoDbSetup
{
    public static async Task SeedDemoDb(IServiceProvider services)
    {
        var userContext = services.GetRequiredService<UserDbContext>();
        var appContext = services.GetRequiredService<TeczterDbContext>();
        
        await DemoDataSeeder.SeedUserData(userContext);
        await DemoDataSeeder.SeedAppData(appContext);
    }
    
    public static async Task CreateDemoDb(IServiceProvider services)
    {
        var userContext = services.GetRequiredService<UserDbContext>();
        var appContext = services.GetRequiredService<TeczterDbContext>();

        await userContext.Database.EnsureDeletedAsync();
        await userContext.Database.EnsureCreatedAsync();
        
        await appContext.Database.EnsureDeletedAsync();
        await appContext.Database.EnsureCreatedAsync();
    }
    
    public static void DemoModeWarning()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("RUNNING IN DEMO MODE USING SQLITE AND SEEDED DATA.");
        Console.ResetColor();
    }
}