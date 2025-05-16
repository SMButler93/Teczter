using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teczter.Data.MiddlewareModels;

namespace Teczter.Data.Configurations;

internal class ErrorLogConfiguration : IEntityTypeConfiguration<ErrorLog>
{
    public void Configure(EntityTypeBuilder<ErrorLog> builder)
    {
        builder.HasOne(x => x.RequestLog)
            .WithOne()
            .HasForeignKey<ErrorLog>(x => x.RequestLogId);
    }
}
