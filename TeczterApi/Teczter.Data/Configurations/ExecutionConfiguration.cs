﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teczter.Domain.Entities;

namespace Teczter.Data.Configurations;

internal class ExecutionConfiguration : IEntityTypeConfiguration<ExecutionEntity>
{
    public void Configure(EntityTypeBuilder<ExecutionEntity> builder)
    {
        builder.Property(x => x.RowVersion)
            .IsRowVersion();
    }
}