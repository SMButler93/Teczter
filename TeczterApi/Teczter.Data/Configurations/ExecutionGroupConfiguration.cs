using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teczter.Domain.Entities;

namespace Teczter.Data.Configurations;

internal class ExecutionGroupConfiguration : IEntityTypeConfiguration<ExecutionGroupEntity>
{
    public void Configure(EntityTypeBuilder<ExecutionGroupEntity> builder)
    {
        builder.Property(x => x.RevisedOn)
            .IsConcurrencyToken();

        builder.HasMany(x => x.Executions)
            .WithOne(y => y.ExecutionGroup)
            .HasForeignKey(y => y.ExecutionGroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}