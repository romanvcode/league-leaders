using LeagueLeaders.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueLeaders.Infrastructure.Database.Configuration;

public class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.ToTable("Players");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Position).HasMaxLength(50);
        builder.Property(p => p.Number);
        builder.Property(p => p.Height);
        builder.Property(p => p.Nationality).HasMaxLength(50);
        builder.Property(p => p.DateOfBirth).HasColumnType("Date");
    }
}
