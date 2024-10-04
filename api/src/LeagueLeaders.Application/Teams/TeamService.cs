using LeagueLeaders.Domain;
using LeagueLeaders.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LeagueLeaders.Application.Teams;

public class TeamService : ITeamService
{
    private readonly LeagueLeadersDbContext _context;

    public TeamService(LeagueLeadersDbContext context)
    {
        _context = context;
    }

    public async Task<Team> GetTeamAsync(int teamId)
    {
        var team = await _context.Teams
            .AsNoTracking()
            .Include(t => t.Players)
            .FirstOrDefaultAsync(t => t.Id == teamId)
            ?? throw new TeamNotFoundException($"Team with Id {teamId} not found.");

        return team;
    }

    public async Task<List<Player>> GetTeamPlayersAsync(int teamId)
    {
        var team = await _context.Teams
            .AsNoTracking()
            .Include(t => t.Players)
            .FirstOrDefaultAsync(t => t.Id == teamId)
            ?? throw new TeamNotFoundException($"Team with Id {teamId} not found.");

        var players = team.Players
            .ToList()
            ?? throw new PlayersNotFoundException($"No players found for the team with ID {teamId}.");

        return players;
    }

    public async Task<List<Match>> GetMatchHistoryForTeamAsync(int teamId, int lastMatches = 5)
    {
        var teamExists = await _context.Teams
            .AsNoTracking()
            .AnyAsync(t => t.Id == teamId);

        if (!teamExists)
        {
            throw new TeamNotFoundException($"Team with Id {teamId} not found.");
        }

        var currentSeason = await _context.Seasons
            .AsNoTracking()
            .Where(s => s.StartAt < DateTime.UtcNow && s.EndAt > DateTime.UtcNow)
            .FirstOrDefaultAsync()
            ?? throw new SeasonNotFoundException($"There is no season which will run during {DateTime.UtcNow}");

        var matches = await _context.Matches
            .AsNoTracking()
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Where(m => m.HomeTeamId == teamId || m.AwayTeamId == teamId)
            .Where(m => m.Stage.SeasonId == currentSeason.Id)
            .Where(m => m.Date < DateTime.UtcNow)
            .OrderByDescending(m => m.Date)
            .Take(lastMatches)
            .ToListAsync()
            ?? throw new MatchesNotFoundException($"No matches found for the team with ID {teamId}.");

        return matches;
    }

    public async Task<List<Team>> GetTeamsBySearchTermAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            throw new ArgumentNullException(message: "Search term cannot be null or empty.", paramName: nameof(searchTerm));
        }

        var teams = await _context.Teams
            .AsNoTracking()
            .ToListAsync();

        var filteredTeams = teams
            .Where(t => t.Name.Split(' ')
                .Any(w => w.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        return filteredTeams;
    }
}