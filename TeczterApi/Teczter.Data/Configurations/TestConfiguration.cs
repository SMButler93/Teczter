using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teczter.Domain.Entities;

namespace Teczter.Data.Configurations;

internal class TestConfiguration : IEntityTypeConfiguration<TestEntity>
{
    public void Configure(EntityTypeBuilder<TestEntity> builder)
    {
        builder.Property(x => x.RevisedOn)
            .IsConcurrencyToken();

        builder.Property(x => x.OwningDepartment)
            .HasConversion<string>();

        builder.HasMany(x => x.TestSteps)
            .WithOne()
            .HasForeignKey(y => y.TestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}