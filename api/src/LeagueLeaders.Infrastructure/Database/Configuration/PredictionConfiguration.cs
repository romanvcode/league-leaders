using LeagueLeaders.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueLeaders.Infrastructure.Database.Configuration;

class PredictionConfiguration : IEntityTypeConfiguration<Prediction>
{
    public void Configure(EntityTypeBuilder<Prediction> builder)
    {
        builder.ToTable("Predictions");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.HomeTeamScore).IsRequired();
        builder.Property(s => s.AwayTeamScore).IsRequired();
        builder
            .HasOne(p => p.Match)
            .WithOne()
            .HasForeignKey<Prediction>(p => p.MatchId)
            .IsRequired();
    }
}
