using LeagueLeaders.Domain;
using LeagueLeaders.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LeagueLeaders.Application
{
    public class LeaderboardService : ILeaderboardService
    {
        private readonly LeagueLeadersDbContext _context;

        public LeaderboardService(LeagueLeadersDbContext context)
        {
            _context = context;
        }

        public async Task<List<Standing>> GetStandingsForEachTeamAsync()
        {
            var currentSeason = await _context.Seasons
                .Where(s => s.StartAt < DateTime.UtcNow && s.EndAt > DateTime.UtcNow)
                .FirstOrDefaultAsync();

            if (currentSeason == null)
            {
                return new List<Standing>();
            }

            var standings = await _context.Standings
                .Include(s => s.Team)
                .Where(s => s.Stage.SeasonId == currentSeason.Id)
                .OrderByDescending(s => s.Points)
                .ToListAsync();

            return standings;
        }
    }
}
