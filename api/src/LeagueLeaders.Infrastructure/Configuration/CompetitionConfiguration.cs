using LeagueLeaders.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueLeaders.Infrastructure.Configuration
{
    public class CompetitionConfiguration : IEntityTypeConfiguration<Competition>
    {
        public void Configure(EntityTypeBuilder<Competition> builder)
        {
            builder.ToTable("Competitions");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).HasMaxLength(100).IsRequired();
            builder.Property(c => c.Region).HasMaxLength(100);
            builder
                .HasMany(c => c.Seasons)
                .WithOne(s => s.Competition)
                .HasForeignKey(s => s.CompetitionId)
                .IsRequired();
        }
    }
}
