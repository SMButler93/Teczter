using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teczter.Domain.Entities;

namespace Teczter.Data.Configurations;

public class ExecutionGroupConfiguration : BaseEntityConfiguration<ExecutionGroupEntity>
{
    public override void Configure(EntityTypeBuilder<ExecutionGroupEntity> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.ExecutionGroupName)
            .IsRequired();

        builder.HasIndex(x => x.ExecutionGroupName)
            .IsUnique();

        builder.HasMany(x => x.Executions)
            .WithOne(y => y.ExecutionGroup)
            .HasForeignKey(y => y.ExecutionGroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}