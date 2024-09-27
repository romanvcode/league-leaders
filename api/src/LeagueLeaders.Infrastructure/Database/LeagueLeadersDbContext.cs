using LeagueLeaders.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LeagueLeaders.Infrastructure.Database;

public class LeagueLeadersDbContext : DbContext
{
    public LeagueLeadersDbContext(DbContextOptions<LeagueLeadersDbContext> options) : base(options)
    {
    }

    public DbSet<Competition> Competitions { get; set; }
    public DbSet<Season> Seasons { get; set; }
    public DbSet<Stage> Stages { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Venue> Venues { get; set; }
    public DbSet<Referee> Referees { get; set; }
    public DbSet<PlayerStat> PlayerStats { get; set; }
    public DbSet<TeamStat> TeamStats { get; set; }
    public DbSet<Standing> Standings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
