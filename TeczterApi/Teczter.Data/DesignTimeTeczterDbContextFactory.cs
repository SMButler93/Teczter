using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Teczter.Data;

public class DesignTimeTeczterDbContextFactory: IDesignTimeDbContextFactory<TeczterDbContext>
{
    public TeczterDbContext CreateDbContext(string[] args)
    {
        var opt = new DbContextOptionsBuilder<TeczterDbContext>()
            .UseNpgsql("Host=localhost;Port=5432;Database=TeczterDb;Username=postgres;Password=Password")
            .Options;
        
        return new TeczterDbContext(opt);
    }
}