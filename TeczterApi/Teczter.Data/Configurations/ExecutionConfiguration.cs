using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teczter.Domain.Entities;

namespace Teczter.Data.Configurations;

internal class ExecutionConfiguration : IEntityTypeConfiguration<ExecutionEntity>
{
    public void Configure(EntityTypeBuilder<ExecutionEntity> builder)
    {
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.CreatedOn)
            .IsRequired();

        builder.Property(x => x.CreatedById)
            .IsRequired();

        builder.Property(x => x.RevisedOn)
            .IsRequired();

        builder.Property(x => x.RevisedOn)
            .IsConcurrencyToken();

        builder.Property(x => x.RevisedById)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .IsRequired();

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.ExecutionGroupId)
            .IsRequired();

        builder.Property(x => x.FailureReason)
            .HasMaxLength(250);
    }
}