namespace Teczter.WebApi.DemoModeConfigAndSetup;

internal static class DemoModeSetup
{
    public static async Task SetupDemoMode(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        await DemoDbSetup.CreateDemoDb(services);
        await DemoDbSetup.SeedDemoDb(services);
    }
}