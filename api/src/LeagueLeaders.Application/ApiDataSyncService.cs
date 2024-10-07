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
    private readonly string _playerPrefix = "sr:player:";
    private readonly string _venuePrefix = "sr:venue:";
    private readonly string _refereePrefix = "sr:referee:";
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
                    EndAt = DateTime.Parse(srSeason.EndDate),
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
                        $"{_seasonPrefix}{season.SportradarId}" == srStage.SeasonId)?.Id ?? 0,
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
                    Abbreviation = srCompetitor.Abbreviation,
                    Country = srCompetitor.Country,
                    Manager = srCompetitor.Manager,
                    Stadium = srCompetitor.Stadium,
                    SportradarId = int.Parse(srCompetitor.Id.ToLower().Replace(_competitorPrefix, ""))
                });
            }
        }

        var srVenues = await _sportradarApiClient.GetVenuesAsync();
        var dbVenues = await _context.Venues.ToListAsync();

        foreach (var srVenue in srVenues)
        {
            var dbVenue = dbVenues.FirstOrDefault(venue => $"{_venuePrefix}{venue.SportradarId}" == srVenue.Id);

            if (dbVenue == null)
            {
                await _context.Venues.AddAsync(new Venue
                {
                    Name = srVenue.Name,
                    City = srVenue.CityName,
                    Capacity = srVenue.Capacity,
                    Country = srVenue.CountryName,
                    SportradarId = int.Parse(srVenue.Id.ToLower().Replace(_venuePrefix, ""))
                });
            }
        }

        var srReferees = await _sportradarApiClient.GetRefereesAsync();
        var dbReferees = await _context.Referees.ToListAsync();

        foreach (var srReferee in srReferees)
        {
            var dbReferee = dbReferees.FirstOrDefault(referee => $"{_refereePrefix}{referee.SportradarId}" == srReferee.Id);

            if (dbReferee == null)
            {
                await _context.Referees.AddAsync(new Referee
                {
                    Name = srReferee.Name,
                    Nationality = srReferee.Nationality,
                    SportradarId = int.Parse(srReferee.Id.ToLower().Replace(_refereePrefix, ""))
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
                        $"{_seasonPrefix}{stage.SeasonId}" == srSportEvent.Id)?.Id ?? 0,
                    HomeTeamId = dbTeams.FirstOrDefault(team =>
                        $"{_competitorPrefix}{team.SportradarId}" == srSportEvent.HomeCompetitorId)?.Id ?? 0,
                    AwayTeamId = dbTeams.FirstOrDefault(team =>
                        $"{_competitorPrefix}{team.SportradarId}" == srSportEvent.AwayCompetitorId)?.Id ?? 0,
                    Date = DateTime.Parse(srSportEvent.Date),
                    VenueId = dbVenues.FirstOrDefault(venue =>
                        $"{_venuePrefix}{venue.SportradarId}" == srSportEvent.VenueId)?.Id ?? 0,
                    RefereeId = dbReferees.FirstOrDefault(referee =>
                        $"{_refereePrefix}{referee.SportradarId}" == srSportEvent.RefereeId)?.Id ?? 0,
                    HomeTeamScore = srSportEvent.HomeCompetitorScore,
                    AwayTeamScore = srSportEvent.AwayCompetitorScore,
                    SportradarId = int.Parse(srSportEvent.Id.ToLower().Replace(_sportEventPrefix, ""))
                });
            }
        }

        var srPlayers = await _sportradarApiClient.GetPlayersAsync();
        var dbPlayers = await _context.Players.ToListAsync();

        foreach (var srPlayer in srPlayers)
        {
            var dbPlayer = dbPlayers.FirstOrDefault(player => player.SportradarId == int.Parse(srPlayer.Id));

            if (dbPlayer == null)
            {
                await _context.Players.AddAsync(new Player
                {
                    Name = srPlayer.Name,
                    Number = srPlayer.JerseyNumber,
                    Position = srPlayer.Type,
                    Height = srPlayer.Height,
                    Nationality = srPlayer.Nationality,
                    DateOfBirth = DateOnly.Parse(srPlayer.DateOfBirth),
                    TeamId = dbTeams.FirstOrDefault(team =>
                        $"{_competitorPrefix}{team.SportradarId}" == srPlayer.CompetitorId)?.Id ?? 0,
                    SportradarId = int.Parse(srPlayer.Id.ToLower().Replace(_sportEventPrefix, ""))
                });
            }
        }

        var lastTenMatchesSportradarIds = srSportEvents
            .Where(se => DateTime.Parse(se.Date) < DateTime.Now)
            .OrderBy(se => se.Date)
            .Select(se => se.Id)
            .Take(10).ToList();

        foreach (var sportEventId in lastTenMatchesSportradarIds)
        {
            var srCompetitorStats = await _sportradarApiClient.GetCompetitorStatsAsync(sportEventId);
            var dbTeamStats = await _context.TeamStats.ToListAsync();

            foreach (var srCompetitorStat in srCompetitorStats)
            {
                var dbTeamStat = dbTeamStats.FirstOrDefault(ts =>
                    ts.TeamId == dbTeams.FirstOrDefault(team =>
                        $"{_competitorPrefix}{team.SportradarId}" == srCompetitorStat.TeamId)?.Id
                    && ts.MatchId == dbMatches.FirstOrDefault(match =>
                        $"{_sportEventPrefix}{match.SportradarId}" == sportEventId)?.Id);

                if (dbTeamStat == null)
                {
                    await _context.TeamStats.AddAsync(new TeamStat
                    {
                        MatchId = dbMatches.FirstOrDefault(match =>
                            $"{_sportEventPrefix}{match.SportradarId}" == sportEventId)?.Id ?? 0,
                        TeamId = dbTeams.FirstOrDefault(team =>
                            $"{_competitorPrefix}{team.SportradarId}" == srCompetitorStat.TeamId)?.Id ?? 0,
                        Possession = srCompetitorStat.Possession,
                        Corners = srCompetitorStat.CornerKicks,
                        Offsides = srCompetitorStat.Offsides,
                        Fouls = srCompetitorStat.Fouls,
                        YellowCards = srCompetitorStat.YellowCards,
                        RedCards = srCompetitorStat.RedCards,
                        Shots = srCompetitorStat.ShotsTotal,
                        ShotsOnTarget = srCompetitorStat.ShotsOnTarget,
                    });
                }

                var srPlayerStats = await _sportradarApiClient.GetPlayerStatsAsync(sportEventId);
                var dbPlayerStats = await _context.PlayerStats.ToListAsync();

                foreach (var srPlayerStat in srPlayerStats)
                {
                    var dbPlayerStat = dbPlayerStats.FirstOrDefault(ps =>
                        ps.PlayerId == dbPlayers.FirstOrDefault(player =>
                            player.SportradarId == int.Parse(srPlayerStat.PlayerId))?.Id
                        && ps.MatchId == dbMatches.FirstOrDefault(match =>
                            $"{_sportEventPrefix}{match.SportradarId}" == sportEventId)?.Id);

                    if (dbPlayerStat == null)
                    {
                        await _context.PlayerStats.AddAsync(new PlayerStat
                        {
                            MatchId = dbMatches.FirstOrDefault(match =>
                                $"{_sportEventPrefix}{match.SportradarId}" == sportEventId)?.Id ?? 0,
                            PlayerId = dbPlayers.FirstOrDefault(player =>
                                player.SportradarId == int.Parse(srPlayerStat.PlayerId))?.Id ?? 0,
                            TeamId = dbTeams.FirstOrDefault(team =>
                                $"{_competitorPrefix}{team.SportradarId}" == srPlayerStat.CompetitorId)?.Id ?? 0,
                            Goals = srPlayerStat.GoalsScored,
                            Assists = srPlayerStat.Assists,
                            Shots = srPlayerStat.ShotsTotal,
                            ShotsOnTarget = srPlayerStat.ShotsOnTarget,
                            YellowCards = srPlayerStat.YellowCards,
                            RedCards = srPlayerStat.RedCards,
                        });
                    }
                }

                var srStandings = await _sportradarApiClient.GetStandingsAsync();
                var dbStandings = await _context.Standings.ToListAsync();

                foreach (var srStanding in srStandings)
                {
                    var dbStanding = dbStandings.FirstOrDefault(standing =>
                        standing.TeamId == dbTeams.FirstOrDefault(team =>
                            $"{_competitorPrefix}{team.SportradarId}" == srStanding.CompetitorId)?.Id);

                    if (dbStanding == null)
                    {
                        await _context.Standings.AddAsync(new Standing
                        {
                            TeamId = dbTeams.FirstOrDefault(team =>
                                $"{_competitorPrefix}{team.SportradarId}" == srStanding.CompetitorId)?.Id ?? 0,
                            StageId = dbStages.FirstOrDefault(stage => srStanding.StageId == stage.Order)?.Id ?? 0,
                            Points = srStanding.Points,
                            Place = srStanding.Rank,
                            MatchesPlayed = srStanding.Played,
                            Wins = srStanding.Win,
                            Draws = srStanding.Draw,
                            Losses = srStanding.Loss,
                            GoalsFor = srStanding.GoalsFor,
                            GoalsAgainst = srStanding.GoalsAgainst,
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}