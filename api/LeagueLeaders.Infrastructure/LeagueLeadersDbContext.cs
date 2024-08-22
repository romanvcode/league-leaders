using Microsoft.EntityFrameworkCore;

namespace LeagueLeaders.Infrastructure
{
    public class LeagueLeadersDbContext : DbContext
    {
        public LeagueLeadersDbContext(DbContextOptions<LeagueLeadersDbContext> options) : base(options)
        {
        }
    }
}
