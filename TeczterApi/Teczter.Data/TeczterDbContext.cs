using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Teczter.Data.MiddlewareModels;
using Teczter.Domain.Entities;

namespace Teczter.Data;

public class TeczterDbContext(DbContextOptions<TeczterDbContext> options) : DbContext(options)
{
    public DbSet<TestEntity> Tests { get; set; }
    public DbSet<TestStepEntity> TestSteps { get; set; }
    public DbSet<ExecutionGroupEntity> ExecutionGroups { get; set; }
    public DbSet<ExecutionEntity> Executions { get; set; }
    public DbSet<UserEntity> Users { get; set; }

    public DbSet<ErrorLog> ErrorLogs { get; set; }
    public DbSet<RequestLog> RequestLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
