using LeagueLeaders.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueLeaders.Infrastructure.Configuration
{
    internal class StandingConfiguration : IEntityTypeConfiguration<Standing>
    {
        public void Configure(EntityTypeBuilder<Standing> builder)
        {
            builder.ToTable("Standings");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Points).HasDefaultValue(0);
            builder.Property(s => s.Place).HasDefaultValue(0);
            builder.Property(s => s.MatchesPlayed).HasDefaultValue(0);
            builder.Property(s => s.Wins).HasDefaultValue(0);
            builder.Property(s => s.Draws).HasDefaultValue(0);
            builder.Property(s => s.Losses).HasDefaultValue(0);
            builder.Property(s => s.GoalsFor).HasDefaultValue(0);
            builder.Property(s => s.GoalsAgainst).HasDefaultValue(0);
            builder.HasOne(s => s.Stage)
                .WithMany()
                .HasForeignKey(s => s.StageId)
                .IsRequired();

            builder.HasOne(s => s.Team)
                .WithMany()
                .HasForeignKey(s => s.TeamId)
                .IsRequired();
        }
    }
}
