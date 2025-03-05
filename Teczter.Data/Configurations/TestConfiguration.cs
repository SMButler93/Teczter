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

        builder.Property(x => x.IsDeleted)
            .IsRequired();

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(75);

        builder.HasIndex(x => x.Title)
            .IsUnique();

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(750);

        builder.HasMany(x => x.TestSteps)
            .WithOne()
            .HasForeignKey(y => y.TestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsMany(x => x.LinkUrls, ownedbuilder =>
        {
            ownedbuilder.Property(y => y.Url)
            .HasColumnName("LinkUrl");
        });
    }
}