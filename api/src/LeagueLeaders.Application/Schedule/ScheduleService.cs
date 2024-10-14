using LeagueLeaders.Domain;
using LeagueLeaders.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace LeagueLeaders.Application.Schedule;

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
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Stage)
            .ToListAsync();

        var closestMatches = matches
            .Where(m => m.Date > DateTime.UtcNow)
            .OrderBy(m => m.Date)
            .Take(5)
            .ToList();

        return closestMatches;
    }
}
