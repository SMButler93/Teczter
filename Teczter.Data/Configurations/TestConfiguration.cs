using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teczter.Domain.Entities;

namespace Teczter.Data.Configurations;

public class TestConfiguration : IEntityTypeConfiguration<TestEntity>
{
    public void Configure(EntityTypeBuilder<TestEntity> builder)
    {
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.CreatedOn)
            .IsRequired();

        builder.Property(x => x.CreatedById)
            .IsRequired();

        builder.Property(x => x.RevisedOn)
            .IsRequired();

        builder.Property(x => x.RevisedById)
            .IsRequired();

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(75);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(750);

        builder.HasMany(x => x.TestSteps)
            .WithOne(y => y.Test)
            .HasForeignKey(y => y.TestId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}