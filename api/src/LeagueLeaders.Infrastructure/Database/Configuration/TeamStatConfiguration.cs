using LeagueLeaders.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueLeaders.Infrastructure.Database.Configuration;

public class TeamStatConfiguration : IEntityTypeConfiguration<TeamStat>
{
    public void Configure(EntityTypeBuilder<TeamStat> builder)
    {
        builder.ToTable("TeamStats");
        builder.HasKey(ts => ts.Id);
        builder.Property(ts => ts.Possession).HasDefaultValue(0);
        builder.Property(ts => ts.RedCards).HasDefaultValue(0);
        builder.Property(ts => ts.YellowCards).HasDefaultValue(0);
        builder.Property(ts => ts.Corners).HasDefaultValue(0);
        builder.Property(ts => ts.Offsides).HasDefaultValue(0);
        builder.Property(ts => ts.Fouls).HasDefaultValue(0);
        builder.Property(ts => ts.Shots).HasDefaultValue(0);
        builder.Property(ts => ts.ShotsOnTarget).HasDefaultValue(0);
        builder
            .HasOne(ts => ts.Match)
            .WithMany()
            .HasForeignKey(ts => ts.MatchId)
            .IsRequired();
        builder
            .HasOne(ts => ts.Team)
            .WithMany()
            .HasForeignKey(ts => ts.TeamId)
            .IsRequired();
    }
}
