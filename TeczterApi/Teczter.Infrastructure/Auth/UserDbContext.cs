using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Teczter.Infrastructure.Auth;

public sealed class UserDbContext(DbContextOptions<UserDbContext> options): IdentityDbContext<TeczterUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("Auth");
    }
}