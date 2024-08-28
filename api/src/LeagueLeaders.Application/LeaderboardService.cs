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
                .AsNoTracking()
                .Where(s => s.StartAt < DateTime.UtcNow && s.EndAt > DateTime.UtcNow)
                .FirstOrDefaultAsync();

            if (currentSeason == null)
            {
                throw new Exception($"There is no season which will run during {DateTime.UtcNow}");
            }

            var standings = await _context.Standings
                .AsNoTracking()
                .Include(s => s.Team)
                .Where(s => s.Stage.SeasonId == currentSeason.Id)
                .OrderBy(s => s.Place)
                .ToListAsync();

            return standings;
        }
    }
}
