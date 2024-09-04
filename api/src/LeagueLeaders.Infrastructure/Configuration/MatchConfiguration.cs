using LeagueLeaders.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueLeaders.Infrastructure.Configuration;

public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.ToTable("Matches");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Date).HasColumnType("Date");
        builder.Property(s => s.HomeTeamScore).HasDefaultValue(0);
        builder.Property(s => s.AwayTeamScore).HasDefaultValue(0);
        builder
            .HasOne(s => s.HomeTeam)
            .WithMany()
            .HasForeignKey(s => s.HomeTeamId)
            .IsRequired();
        builder
            .HasOne(s => s.AwayTeam)
            .WithMany()
            .HasForeignKey(s => s.AwayTeamId)
            .IsRequired();
        builder
            .HasOne(s => s.Venue)
            .WithMany()
            .HasForeignKey(s => s.VenueId)
            .IsRequired();
        builder
            .HasOne(s => s.Referee)
            .WithMany()
            .HasForeignKey(s => s.RefereeId)
            .IsRequired();
    }
}
