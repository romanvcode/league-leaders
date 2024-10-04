using LeagueLeaders.Domain;
using LeagueLeaders.Infrastructure.Clients.SportradarApi;
using LeagueLeaders.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace LeagueLeaders.Application;

public class ApiDataSyncService
{
    private readonly LeagueLeadersDbContext _context;
    private readonly ISportradarApiClient _sportradarApiClient;
    private readonly string _competitionPrefix = "sr:competition:";
    private readonly string _seasonPrefix = "sr:season:";
    private readonly string _competitorPrefix = "sr:competitor:";
    private readonly string _sportEventPrefix = "sr:sport_event:";

    public ApiDataSyncService(LeagueLeadersDbContext context, ISportradarApiClient sportradarApiClient)
    {
        _context = context;
        _sportradarApiClient = sportradarApiClient;
    }

    public async Task SyncDataAsync()
    {
        var srCompetition = await _sportradarApiClient.GetCompetitionAsync();
        var dbCompetition = await _context.Competitions.FirstOrDefaultAsync
            (competiton => $"{_competitionPrefix}{competiton.SportradarId}" == srCompetition.Id);

        if (dbCompetition == null)
        {
            await _context.Competitions.AddAsync(new Competition
            {
                Name = srCompetition.Name,
                SportradarId = int.Parse(srCompetition.Id.ToLower().Replace(_competitionPrefix, ""))
            });
        }

        var srSeasons = await _sportradarApiClient.GetSeasonsAsync();
        var dbSeasons = await _context.Seasons.ToListAsync();

        foreach (var srSeason in srSeasons)
        {
            var dbSeason = dbSeasons.FirstOrDefault(season => $"{_seasonPrefix}{season.SportradarId}" == srSeason.Id);

            if (dbSeason == null)
            {
                await _context.Seasons.AddAsync(new Season
                {
                    Name = srSeason.Name,
                    SportradarId = int.Parse(srSeason.Id.ToLower().Replace(_competitionPrefix, "")),
                    CompetitionId = dbCompetition?.Id ?? 0,
                    StartAt = DateTime.Parse(srSeason.StartDate),
                    EndAt = DateTime.Parse(srSeason.EndDate)
                });
            }
        }

        var srStages = await _sportradarApiClient.GetStagesAsync();
        var dbStages = await _context.Stages.ToListAsync();
        foreach (var srStage in srStages)
        {
            var dbStage = dbStages.FirstOrDefault(stage => stage.Order == srStage.Order);

            if (dbStage == null)
            {
                await _context.Stages.AddAsync(new Stage
                {
                    Name = srStage.Phase,
                    Type = srStage.Type,
                    SeasonId = dbSeasons.FirstOrDefault(season =>
                        $"{_seasonPrefix}{season.SportradarId}" == srStage.SeasonId)?.Id 
                        ?? 0,
                    Order = srStage.Order,
                });
            }
        }

        var srCompetitors = await _sportradarApiClient.GetCompetitorsAsync();
        var dbTeams = await _context.Teams.ToListAsync();

        foreach (var srCompetitor in srCompetitors)
        {
            var dbTeam = dbTeams.FirstOrDefault(team => $"{_competitorPrefix}{team.SportradarId}" == srCompetitor.Id);

            if (dbTeam == null)
            {
                await _context.Teams.AddAsync(new Team
                {
                    Name = srCompetitor.Name,
                    SportradarId = int.Parse(srCompetitor.Id.ToLower().Replace(_competitorPrefix, ""))
                });
            }
        }

        var srSportEvents = await _sportradarApiClient.GetSportEventsAsync();
        var dbMatches = await _context.Matches.ToListAsync();

        foreach (var srSportEvent in srSportEvents)
        {
            var dbMatch = dbMatches.FirstOrDefault(match => $"{_sportEventPrefix}{match.SportradarId}" == srSportEvent.Id);

            if (dbMatch == null)
            {
                await _context.Matches.AddAsync(new Match
                {
                    StageId = dbStages.FirstOrDefault(stage =>
                        $"{_seasonPrefix}{stage.SeasonId}" == srSportEvent.Id)?.Id
                        ?? 0,
                    HomeTeamId = dbTeams.FirstOrDefault(team =>
                        $"{_competitorPrefix}{team.SportradarId}" == srSportEvent.HomeCompetitorId)?.Id
                        ?? 0,
                    AwayTeamId = dbTeams.FirstOrDefault(team =>
                        $"{_competitorPrefix}{team.SportradarId}" == srSportEvent.AwayCompetitorId)?.Id
                        ?? 0,
                    Date = DateTime.Parse(srSportEvent.Date),
                    VenueId = 0,
                    RefereeId = 0,
                    HomeTeamScore = srSportEvent.HomeCompetitorScore,
                    AwayTeamScore = srSportEvent.AwayCompetitorScore,
                    SportradarId = int.Parse(srSportEvent.Id.ToLower().Replace(_sportEventPrefix, ""))
                });
            }
        }
        // ...

        await _context.SaveChangesAsync();
    }
}
