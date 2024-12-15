using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teczter.Domain.Entities;

namespace Teczter.Data.Configurations;

public class TestConfiguration : IEntityTypeConfiguration<TestEntity>
{
    public void Configure(EntityTypeBuilder<TestEntity> builder)
    {
        builder.Property(x => x.CreatedOn)
            .IsRequired();

        builder.Property(x => x.CreatedById)
            .IsRequired();

        builder.Property(x => x.RevisedOn)
            .IsRequired();

        builder.Property(x => x.RevisedById)
            .IsRequired();

        builder.Property(x => x.TestingRoundId)
            .IsRequired();

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(75);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(750);

        builder.Property(x => x.TestState)
            .IsRequired();

        builder.HasMany(x => x.TestSteps)
            .WithOne(y => y.Test)
            .HasForeignKey(y => y.TestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.TestRound)
            .WithMany(y => y.Tests)
            .HasForeignKey(x => x.TestingRoundId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.AssignedUser)
            .WithMany(y => y.AssignedTests)
            .HasForeignKey(x => x.AssignedUserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}