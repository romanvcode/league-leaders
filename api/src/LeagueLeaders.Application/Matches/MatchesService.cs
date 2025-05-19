using LeagueLeaders.Infrastructure.Database;
using LeagueLeaders.Domain;
using Microsoft.EntityFrameworkCore;
using LeagueLeaders.Infrastructure.Clients.SportradarApi;
using Microsoft.Extensions.Options;

namespace LeagueLeaders.Application.Matches;

public class MatchesService : IMatchesService
{
    private readonly LeagueLeadersDbContext _context;
    private readonly ISportradarApiClient _apiClient;
    private readonly string _sportEventPrefix;
    private readonly string _competitorPrefix;

    public MatchesService(
        LeagueLeadersDbContext context,
        ISportradarApiClient apiClient,
        IOptions<SportradarSettings> options)
    {
        _context = context;
        _apiClient = apiClient;
        _sportEventPrefix = options.Value.SportEventPrefix;
        _competitorPrefix = options.Value.CompetitorPrefix;
    }

    public async Task<List<TeamStat>> GetTeamStatsForMatchAsync(int matchId)
    {
        var dbTeamsStats = await _context.TeamStats
            .AsNoTracking()
            .Where(ts => ts.MatchId == matchId)
            .ToListAsync();

        if (dbTeamsStats.Count > 0)
        {
            return dbTeamsStats;
        }

        var match = await _context.Matches
            .AsNoTracking()
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .FirstOrDefaultAsync(m => m.Id == matchId)
            ?? throw new MatchesNotFoundException($"Match with Id {matchId} not found.");

        var competitorsStats = await _apiClient.GetCompetitorStatsAsync(_sportEventPrefix + match.SportradarId)
            ?? throw new TeamStatsNotFoundException($"No team stats found for match with Id {matchId}.");

        var teamsStats = new List<TeamStat>();
        foreach(var competitorStat in competitorsStats)
        {
            var teamId = _context.Teams.FirstOrDefault(team =>
                _competitorPrefix + team.SportradarId == competitorStat.TeamId)?.Id ?? 0;

            var teamStat = new TeamStat
            {
                MatchId = matchId,
                TeamId = teamId,
                Possession = competitorStat.Possession,
                Corners = competitorStat.CornerKicks,
                Offsides = competitorStat.Offsides,
                Fouls = competitorStat.Fouls,
                YellowCards = competitorStat.YellowCards,
                RedCards = competitorStat.RedCards,
                Shots = competitorStat.ShotsTotal,
                ShotsOnTarget = competitorStat.ShotsOnTarget,
            };
            teamsStats.Add(teamStat);
        }

        var teamStatsList = teamsStats.AsList();

        await _context.TeamStats.AddRangeAsync(teamStatsList);
        await _context.SaveChangesAsync();

        return teamStatsList;
    }
}
