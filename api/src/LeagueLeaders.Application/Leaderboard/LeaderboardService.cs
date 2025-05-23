﻿using LeagueLeaders.Domain;
using LeagueLeaders.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace LeagueLeaders.Application.Leaderboard;

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
            .FirstOrDefaultAsync()
            ?? throw new SeasonNotFoundException($"There is no season which will run during {DateTime.UtcNow}");

        var standings = await _context.Standings
            .AsNoTracking()
            .Include(s => s.Team)
            .Where(s => s.Stage.SeasonId == currentSeason.Id)
            .OrderByDescending(s => s.Points)
            .ToListAsync();

        if (standings == null || standings.Count == 0)
            throw new StandingsNotFoundException("No standings found.");

        return standings;
    }
}