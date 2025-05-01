using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teczter.Domain.Entities;

namespace Teczter.Data.Configurations;

internal class ExecutionGroupConfiguration : IEntityTypeConfiguration<ExecutionGroupEntity>
{
    public void Configure(EntityTypeBuilder<ExecutionGroupEntity> builder)
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
            .IsRequired(); ;

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

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