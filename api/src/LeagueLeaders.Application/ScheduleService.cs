using LeagueLeaders.Domain;
using LeagueLeaders.Infrastructure;
using Microsoft.EntityFrameworkCore;
using LeagueLeaders.Application.Exceptions;

namespace LeagueLeaders.Application
{
    public class ScheduleService : IScheduleService
    {
        private readonly LeagueLeadersDbContext _context;

        public ScheduleService(LeagueLeadersDbContext context)
        {
            _context = context;
        }

        public async Task<List<Match>> GetClosestMatchesAsync()
        {
            var currentSeason = await _context.Seasons
                .AsNoTracking()
                .Where(s => s.StartAt < DateTime.UtcNow && s.EndAt > DateTime.UtcNow)
                .FirstOrDefaultAsync()
                ?? throw new SeasonNotFoundException($"There is no season which will run during {DateTime.UtcNow}");

            var matches = await _context.Matches
                .AsNoTracking()
                .Where(m => m.Stage.SeasonId == currentSeason.Id)
                .Include(m => m.Stage)
                .ToListAsync();

            var currentStage = matches
                .GroupBy(m => m.Stage)
                .OrderByDescending(g => g.Key.Id)
                .Select(g => g.Key)
                .FirstOrDefault()
                ?? throw new StageNotFoundException($"There is no stage which will run during current season: {currentSeason.Name}");

            var closestMatches = matches
                .Where(m => m.StageId == currentStage.Id)
                .Where(m => m.Date > DateTime.UtcNow)
                .OrderBy(m => m.Date)
                .Take(5)
                .ToList();

            return closestMatches;
        }
    }
}
