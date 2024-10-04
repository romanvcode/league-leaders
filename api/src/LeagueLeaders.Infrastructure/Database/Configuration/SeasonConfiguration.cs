using LeagueLeaders.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueLeaders.Infrastructure.Database.Configuration;

public class SeasonConfiguration : IEntityTypeConfiguration<Season>
{
    public void Configure(EntityTypeBuilder<Season> builder)
    {
        builder.ToTable("Seasons");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Name).HasMaxLength(100).IsRequired();
        builder.Property(s => s.StartAt).HasColumnType("Date");
        builder.Property(s => s.EndAt).HasColumnType("Date");
        builder
            .HasMany(s => s.Stages)
            .WithOne(s => s.Season)
            .HasForeignKey(s => s.SeasonId)
            .IsRequired();
    }
}
