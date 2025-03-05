using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teczter.Domain.Entities;

namespace Teczter.Data.Configurations;

public class TestStepConfiguration : IEntityTypeConfiguration<TestStepEntity>
{
    public void Configure(EntityTypeBuilder<TestStepEntity> builder)
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

        builder.Property(x => x.TestId)
            .IsRequired();

        builder.Property(x => x.StepPlacement)
            .IsRequired();

        builder.Property(x => x.Instructions)
            .IsRequired()
            .HasMaxLength(750);

        builder.OwnsMany(x => x.LinkUrls, ownedbuilder =>
        {
            ownedbuilder.Property(y => y.Url)
            .HasColumnName("LinkUrl");
        });
    }
}
