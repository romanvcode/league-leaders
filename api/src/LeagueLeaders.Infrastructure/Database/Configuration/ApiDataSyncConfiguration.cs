﻿using LeagueLeaders.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueLeaders.Infrastructure.Database.Configuration;

public class ApiDataSyncConfiguration : IEntityTypeConfiguration<ApiDataSyncLog>
{
    public void Configure(EntityTypeBuilder<ApiDataSyncLog> builder)
    {
        builder.ToTable("ApiDataSyncLogs");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Source).HasMaxLength(100).IsRequired();
        builder.Property(a => a.SyncTime).IsRequired();
        builder.Property(a => a.IsSuccess).IsRequired();
        builder.Property(a => a.ErrorMessage).HasMaxLength(1000);
    }
}
