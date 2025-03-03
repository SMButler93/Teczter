using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teczter.Domain.Entities;

namespace Teczter.Data.Configurations;

public class ExecutionConfiguration : BaseEntityConfiguration<ExecutionEntity>
{
    public override void Configure(EntityTypeBuilder<ExecutionEntity> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.ExecutionGroupId)
            .IsRequired();

        builder.Property(x => x.FailureReason)
            .HasMaxLength(250);
    }
}
