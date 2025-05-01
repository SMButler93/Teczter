using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teczter.Data.MiddlewareModels;

namespace Teczter.Data.Configurations;

internal class RequestLogConfiguration : IEntityTypeConfiguration<RequestLog>
{
    public void Configure(EntityTypeBuilder<RequestLog> builder)
    {
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.TimeStamp)
            .IsRequired();
    }
}
