using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teczter.Domain.Entities;

namespace Teczter.Data.Configurations;

public class TestConfiguration : BaseEntityConfiguration<TestEntity>
{
    public override void Configure(EntityTypeBuilder<TestEntity> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(75);

        builder.HasIndex(x => x.Title)
            .IsUnique();

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(750);

        builder.HasMany(x => x.TestSteps)
            .WithOne(y => y.Test)
            .HasForeignKey(y => y.TestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsMany(x => x.LinkUrls, ownedbuilder =>
        {
            ownedbuilder.Property(y => y.Url)
            .HasColumnName("LinkUrl");
        });
    }
}