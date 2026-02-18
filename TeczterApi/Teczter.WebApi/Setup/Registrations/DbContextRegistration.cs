using Microsoft.EntityFrameworkCore;
using Teczter.Data;
using Teczter.Infrastructure.Auth;

namespace Teczter.WebApi.Registrations;

public static class DbContextRegistration
{
    public static void AddDbContexts(
        this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        var isDemoMode = configuration.GetValue<bool>("demo");

        services.AddDbContext<TeczterDbContext>(options =>
        {
            if (isDemoMode)
            {
                options.UseSqlite(configuration.GetConnectionString("TeczterDemoDb"));
            }
            else
            {
                options.UseNpgsql(configuration.GetConnectionString("TeczterDb"),
                        npgsql =>
                        {
                            npgsql.MigrationsAssembly(typeof(TeczterDbContext).Assembly.FullName!);
                            npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "appMigrations");
                        })
                    .UseLazyLoadingProxies();
            }
            
            if (environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging()
                    .LogTo(Console.WriteLine);
            }
        });
        
        services.AddDbContext<UserDbContext>(options =>
        {
            if (isDemoMode)
            {
                options.UseSqlite(configuration.GetConnectionString("TeczterDemoDb"));
            }
            else
            {
                options.UseNpgsql(configuration.GetConnectionString("TeczterDb"),
                        npgsql =>
                        {
                            npgsql.MigrationsAssembly(typeof(UserDbContext).Assembly.FullName!);
                            npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "userMigrations");
                        })
                    .UseLazyLoadingProxies();
            }
            
            if (environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging()
                    .LogTo(Console.WriteLine);
            }
        });
    }
}