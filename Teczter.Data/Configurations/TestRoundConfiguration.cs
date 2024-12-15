using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teczter.Domain.Entities;

namespace Teczter.Data.Configurations;

public class TestRoundConfiguration : IEntityTypeConfiguration<TestRoundEntity>
{
    public void Configure(EntityTypeBuilder<TestRoundEntity> builder)
    {
        builder.Property(x => x.CreatedOn)
            .IsRequired();

        builder.Property(x => x.CreatedById)
            .IsRequired();

        builder.Property(x => x.RevisedOn)
            .IsRequired();

        builder.Property(x => x.RevisedById)
            .IsRequired();

        builder.Property(x => x.TestRoundName)
            .IsRequired();

        builder.HasIndex(x => x.TestRoundName)
            .IsUnique();

        builder.HasMany(x => x.Tests)
            .WithOne(y => y.TestRound)
            .HasForeignKey(y => y.TestingRoundId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}