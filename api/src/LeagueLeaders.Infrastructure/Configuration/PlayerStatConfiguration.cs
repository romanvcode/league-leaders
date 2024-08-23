using LeagueLeaders.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueLeaders.Infrastructure.Configuration
{
    public class PlayerStatConfiguration : IEntityTypeConfiguration<PlayerStat>
    {
        public void Configure(EntityTypeBuilder<PlayerStat> builder)
        {
            builder.ToTable("PlayerStats");
            builder.HasKey(ps => ps.Id);
            builder.Property(ps => ps.Goals).HasDefaultValue(0);
            builder.Property(ps => ps.Assists).HasDefaultValue(0);
            builder.Property(ps => ps.RedCards).HasDefaultValue(0);
            builder.Property(ps => ps.YellowCards).HasDefaultValue(0);
            builder.Property(ps => ps.Shots).HasDefaultValue(0);
            builder.Property(ps => ps.ShotsOnTarget).HasDefaultValue(0);
            builder
                .HasOne(ps => ps.Match)
                .WithMany()
                .HasForeignKey(ps => ps.MatchId)
                .IsRequired();
            builder
                .HasOne(ps => ps.Player)
                .WithMany()
                .HasForeignKey(ps => ps.PlayerId)
                .IsRequired();
            builder
                .HasOne(ps => ps.Team)
                .WithMany()
                .HasForeignKey(ps => ps.TeamId)
                .IsRequired();
        }
    }
}
