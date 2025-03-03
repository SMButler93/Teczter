using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teczter.Domain.Entities;

namespace Teczter.Data.Configurations;

public class TestStepConfiguration : BaseEntityConfiguration<TestStepEntity>
{
    public override void Configure(EntityTypeBuilder<TestStepEntity> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.TestId)
            .IsRequired();

        builder.Property(x => x.StepPlacement)
            .IsRequired();

        builder.Property(x => x.Instructions)
            .IsRequired()
            .HasMaxLength(750);

        builder.HasOne(x => x.Test)
            .WithMany(y => y.TestSteps)
            .HasForeignKey(x => x.TestId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.OwnsMany(x => x.LinkUrls, ownedbuilder =>
        {
            ownedbuilder.Property(y => y.Url)
            .HasColumnName("LinkUrl");
        });
    }
}
