using LeagueLeaders.Domain;
using LeagueLeaders.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LeagueLeaders.Application
{
    public class ScheduleSerivce : IScheduleSerivce
    {
        private readonly LeagueLeadersDbContext _context;

        public ScheduleSerivce(LeagueLeadersDbContext context)
        {
            _context = context;
        }

        public async Task<List<Match>> GetClosestMatchesAsync()
        {
            var currentSeason = await _context.Seasons
                .Where(s => s.StartAt < DateTime.UtcNow && s.EndAt > DateTime.UtcNow)
                .FirstOrDefaultAsync();

            if (currentSeason == null)
            {
                throw new Exception($"There is no season which will run during {DateTime.UtcNow}");
            }

            var currentStage = await _context.Matches
                .Where(m => m.Stage.SeasonId == currentSeason.Id)
                .Include(m => m.Stage)
                .GroupBy(m => m.Stage)
                .OrderByDescending(g => g.Key)
                .Select(g => g.Key)
                .FirstOrDefaultAsync();

            if (currentStage == null)
            {
                throw new Exception($"There is no stage which will run during current season: {currentSeason.Name}");
            }

            var matches = await _context.Matches
                .Where(m => m.StageId == currentStage.Id)
                .OrderBy(m => m.Date)
                .Take(5)
                .ToListAsync();

            return matches;
        }
    }
}
