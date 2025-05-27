using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teczter.Domain.Entities;

namespace Teczter.Data.Configurations;

internal class UserConfigurations : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.Property(x => x.RowVersion)
            .IsRowVersion();

        builder.HasMany(x => x.AssignedExcutions)
            .WithOne(y => y.AssignedUser)
            .HasForeignKey(y => y.AssignedUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}