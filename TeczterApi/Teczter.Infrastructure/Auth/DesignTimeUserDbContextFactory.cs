using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;

namespace Teczter.Infrastructure.Auth;

public class DesignTimeUserDbContextFactory() : IDesignTimeDbContextFactory<UserDbContext>
{
    public UserDbContext CreateDbContext(string[] args)
    {
        var opt = new DbContextOptionsBuilder<UserDbContext>()
            .UseNpgsql("Host=localhost;Port=5432;Database=TeczterDb;Username=postgres;Password=Password")
            .Options;

        return new UserDbContext(opt);
    }
}