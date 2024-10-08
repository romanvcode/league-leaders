using LeagueLeaders.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueLeaders.Infrastructure.Database.Configuration;

public class SyncLogConfiguration : IEntityTypeConfiguration<SyncLog>
{
    public void Configure(EntityTypeBuilder<SyncLog> builder)
    {
        builder.ToTable("ApiDataSyncLogs");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Client).HasMaxLength(100).IsRequired();
        builder.Property(a => a.SyncTime).IsRequired();
        builder.Property(a => a.Status).IsRequired();
        builder.Property(a => a.Reason).HasMaxLength(1000);
    }
}
