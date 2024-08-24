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
            var currentStageId = await _context.Matches
                .Include(m => m.Stage)
                .GroupBy(m => m.StageId)
                .OrderByDescending(g => g.Key)
                .Select(g => g.Key)
                .FirstOrDefaultAsync();

            if (currentStageId == default)
            {
                return new List<Match>();
            }

            var matches = await _context.Matches
                .Where(m => m.StageId == currentStageId)
                .OrderBy(m => m.Date)
                .Take(5)
                .ToListAsync();

            return matches;
        }
    }
}
