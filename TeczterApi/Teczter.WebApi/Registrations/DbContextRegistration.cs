using Microsoft.EntityFrameworkCore;
using Teczter.Data;
using Teczter.Infrastructure.Auth;

namespace Teczter.WebApi.Registrations;

public static class DbContextRegistration
{
    public static IServiceCollection AddDbContexts(
        this IServiceCollection services, ConfigurationManager configuration, bool isDevelopmentEnvironment)
    {
        services.AddDbContext<TeczterDbContext>(options =>
        {
            ConfigureOptions(options, configuration, isDevelopmentEnvironment);
        });

        services.AddDbContext<UserDbContext>(options =>
        {
            ConfigureOptions(options, configuration, isDevelopmentEnvironment);
        });

        return services;
    }

    private static void ConfigureOptions(DbContextOptionsBuilder options, ConfigurationManager configuration, bool isDevelopmentEnvironment)
    {
        options.UseNpgsql(configuration.GetConnectionString("TeczterDb"),
                npgsql => npgsql.MigrationsAssembly("Teczter.Data"))
            .UseLazyLoadingProxies();

        if (isDevelopmentEnvironment)
        {
            options.EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine);
        }
    }
}