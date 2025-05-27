using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teczter.Domain.Entities;

namespace Teczter.Data.Configurations;

internal class TestStepConfiguration : IEntityTypeConfiguration<TestStepEntity>
{
    public void Configure(EntityTypeBuilder<TestStepEntity> builder)
    {
        builder.Property(x => x.RowVersion)
            .IsRowVersion();
    }
}
