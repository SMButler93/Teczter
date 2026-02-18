using Microsoft.AspNetCore.Identity;
using Teczter.Infrastructure.Auth;

namespace Teczter.WebApi.Setup.Registrations;

public static class IdentityRegistration
{
    public static void AddTeczterIdentity(this IServiceCollection services)
    {
        services.AddIdentityCore<TeczterUser>(opt =>
            {
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequiredLength = 8;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<UserDbContext>()
            .AddDefaultTokenProviders();
    }   
}