using LeagueLeaders.Domain;
using LeagueLeaders.Infrastructure.Clients.SportradarApi;
using LeagueLeaders.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace LeagueLeaders.Application.ApiDataSync;

public class ApiDataSyncService : IApiDataSyncService
{
    private readonly LeagueLeadersDbContext _context;
    private readonly ISportradarApiClient _sportradarApiClient;
    private readonly string _competitionPrefix;
    private readonly string _seasonPrefix;
    private readonly string _competitorPrefix;
    private readonly string _playerPrefix;
    private readonly string _venuePrefix;
    private readonly string _refereePrefix;
    private readonly string _sportEventPrefix;

    public ApiDataSyncService(
        LeagueLeadersDbContext context,
        ISportradarApiClient sportradarApiClient,
        IOptions<SportradarSettings> options)
    {
        _context = context;
        _sportradarApiClient = sportradarApiClient;
        _competitionPrefix = options.Value.CompetitionPrefix;
        _seasonPrefix = options.Value.SeasonPrefix;
        _competitorPrefix = options.Value.CompetitorPrefix;
        _playerPrefix = options.Value.PlayerPrefix;
        _venuePrefix = options.Value.VenuePrefix;
        _refereePrefix = options.Value.RefereePrefix;
        _sportEventPrefix = options.Value.SportEventPrefix;
    }

    public async Task ReportSuccessfulSyncronizationAsync(string client)
    {
        var log = new SyncLog
        {
            Client = client,
            SyncTime = DateTime.UtcNow,
            Status = "Successful",
        };

        await _context.SyncLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }

    public async Task ReportFailedSyncronizationAsync(string client, string reason)
    {
        var log = new SyncLog
        {
            Client = client,
            SyncTime = DateTime.UtcNow,
            Status = "Failed",
            Reason = reason,
        };

        await _context.SyncLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }

    public async Task SyncDataAsync()
    {
        var competition = await _context.Competitions.SingleAsync();
        var seasons = await _context.Seasons.AsNoTracking().ToListAsync();
        var stages = await _context.Stages.AsNoTracking().ToListAsync();
        var teams = await _context.Teams.AsNoTracking().ToListAsync();
        var venues = await _context.Venues.AsNoTracking().ToListAsync();
        var referees = await _context.Referees.AsNoTracking().ToListAsync();
        var players = await _context.Players.AsNoTracking().ToListAsync();
        var matches = await _context.Matches.AsNoTracking().ToListAsync();

        await SyncCompetitionAsync();
        await SyncSeasonsAsync(competition);
        await SyncStagesAsync(seasons);
        await SyncTeamsAsync();
        await SyncVenuesAsync();
        await SyncRefereesAsync();
        await SyncPlayersAsync(teams);
        await SyncMatchesAsync(stages, teams, venues, referees);
        await SyncStatsAsync(players, teams, matches);
        await SyncStandingsAsync(teams, stages);
    }

    private async Task SyncCompetitionAsync()
    {
        var srCompetition = await _sportradarApiClient.GetCompetitionAsync();

        var dbCompetition = await _context.Competitions.FirstOrDefaultAsync
            (competiton => _competitionPrefix + competiton.SportradarId == srCompetition.Id);

        if (dbCompetition == null)
        {
            var isParsed = int.TryParse(srCompetition.Id.ToLower().Replace(_competitionPrefix, ""), out int sportradarId);
            if (!isParsed)
            {
                throw new InvalidOperationException("Sportradar ID is not in the correct format.");
            }

            dbCompetition = new Competition
            {
                Name = srCompetition.Name,
                Region = srCompetition.Region,
                SportradarId = sportradarId
            };

            await _context.Competitions.AddAsync(dbCompetition);
        }

        await _context.SaveChangesAsync();
    }

    private async Task SyncSeasonsAsync(Competition dbCompetition)
    {
        var srSeasons = await _sportradarApiClient.GetSeasonsAsync();
        var dbSeasons = await _context.Seasons.ToListAsync();

        var seasons = new List<Season>();
        foreach (var srSeason in srSeasons)
        {
            var dbSeason = dbSeasons.FirstOrDefault(season => _seasonPrefix + season.SportradarId == srSeason.Id);

            if (dbSeason == null)
            {
                var isSportradaarIdParsed = int.TryParse(srSeason.Id.ToLower().Replace(_seasonPrefix, ""), out int sportradarId);
                if (!isSportradaarIdParsed)
                    throw new InvalidOperationException("Sportradar ID is not in the correct format.");

                var isStartDateParsed = DateTime.TryParse(srSeason.StartDate, out DateTime startDate);
                if (!isStartDateParsed)
                    throw new InvalidOperationException("Start date is not in the correct format.");

                var isEndDateParsed = DateTime.TryParse(srSeason.EndDate, out DateTime endDate);
                if (!isEndDateParsed)
                    throw new InvalidOperationException("End date is not in the correct format.");

                seasons.Add(new Season
                {
                    Name = srSeason.Name,
                    SportradarId = sportradarId,
                    CompetitionId = dbCompetition?.Id ?? 0,
                    StartAt = startDate,
                    EndAt = endDate
                });
            }
        }
        await _context.Seasons.AddRangeAsync(seasons);
        await _context.SaveChangesAsync();
    }

    private async Task SyncStagesAsync(List<Season> dbSeasons)
    {
        var srStages = await _sportradarApiClient.GetStagesAsync();
        var dbStages = await _context.Stages.ToListAsync();

        var stages = new List<Stage>();
        foreach (var srStage in srStages)
        {
            var dbStage = dbStages.FirstOrDefault(stage => stage.StageOrder == srStage.Order);

            if (dbStage == null)
            {
                var seasonId = dbSeasons.FirstOrDefault(season => 
                    _seasonPrefix + season.SportradarId == srStage.SeasonId)?.Id ?? 0;
                if (seasonId == 0)
                    throw new InvalidOperationException("Season ID is not found.");

                stages.Add(new Stage
                {
                    Name = srStage.Phase,
                    Type = srStage.Type,
                    SeasonId = seasonId,
                    StageOrder = srStage.Order,
                });
            }
        }
        await _context.Stages.AddRangeAsync(stages);
        await _context.SaveChangesAsync();
    }

    private async Task SyncTeamsAsync()
    {
        var srCompetitors = await _sportradarApiClient.GetCompetitorsAsync();
        var dbTeams = await _context.Teams.ToListAsync();

        var teams = new List<Team>();
        foreach (var srCompetitor in srCompetitors)
        {
            var dbTeam = dbTeams.FirstOrDefault(team => 
                _competitorPrefix + team.SportradarId == srCompetitor.Id);

            if (dbTeam == null)
            {
                var isSportradarIdParsed = int.TryParse(srCompetitor.Id.ToLower().Replace(_competitorPrefix, ""), out int sportradarId);
                if (!isSportradarIdParsed)
                    throw new InvalidOperationException("Sportradar ID is not in the correct format.");

                teams.Add(new Team
                {
                    Name = srCompetitor.Name,
                    Abbreviation = srCompetitor.Abbreviation,
                    Country = srCompetitor.Country,
                    Manager = srCompetitor.Manager,
                    Stadium = srCompetitor.Stadium,
                    SportradarId = sportradarId
                });
            }
        }
        await _context.Teams.AddRangeAsync(teams);
        await _context.SaveChangesAsync();
    }

    private async Task SyncVenuesAsync()
    {
        var srVenues = await _sportradarApiClient.GetVenuesAsync();
        var dbVenues = await _context.Venues.ToListAsync();

        var venues = new List<Venue>();
        foreach (var srVenue in srVenues)
        {
            var dbVenue = dbVenues.FirstOrDefault(venue => 
                _venuePrefix + venue.SportradarId == srVenue.Id);

            if (dbVenue == null)
            {
                var isSportradarIdParsed = int.TryParse(srVenue.Id.ToLower().Replace(_venuePrefix, ""), out int sportradarId);
                if (!isSportradarIdParsed)
                    throw new InvalidOperationException("Sportradar ID is not in the correct format.");

                venues.Add(new Venue
                {
                    Name = srVenue.Name,
                    City = srVenue.CityName,
                    Capacity = srVenue.Capacity,
                    Country = srVenue.CountryName,
                    SportradarId = sportradarId
                });
            }
        }
        await _context.Venues.AddRangeAsync(venues);
        await _context.SaveChangesAsync();
    }

    private async Task SyncRefereesAsync()
    {
        var srReferees = await _sportradarApiClient.GetRefereesAsync();
        var dbReferees = await _context.Referees.ToListAsync();

        var referees = new List<Referee>();
        foreach (var srReferee in srReferees)
        {
            var dbReferee = dbReferees.FirstOrDefault(referee => 
                _refereePrefix + referee.SportradarId == srReferee.Id);

            if (dbReferee == null)
            {
                var isSportradarIdParsed = int.TryParse(srReferee.Id.ToLower().Replace(_refereePrefix, ""), out int sportradarId);
                if (!isSportradarIdParsed)
                    throw new InvalidOperationException("Sportradar ID is not in the correct format.");

                referees.Add(new Referee
                {
                    Name = srReferee.Name,
                    Nationality = srReferee.Nationality,
                    SportradarId = sportradarId
                });
            }
        }
        await _context.Referees.AddRangeAsync(referees);
        await _context.SaveChangesAsync();
    }

    public async Task SyncMatchesAsync(List<Stage> dbStages, List<Team> dbTeams, List<Venue> dbVenues, List<Referee> dbReferees)
    {
        var srSportEvents = await _sportradarApiClient.GetSportEventsAsync();
        var dbMatches = await _context.Matches.AsNoTracking().ToListAsync();

        var matches = new List<Match>();
        foreach (var srSportEvent in srSportEvents)
        {
            var dbMatch = dbMatches.FirstOrDefault(match => 
                _sportEventPrefix + match.SportradarId == srSportEvent.Id);

            if (dbMatch == null)
            {
                var isSportradarIdParsed = int.TryParse(srSportEvent.Id.ToLower().Replace(_sportEventPrefix, ""), out int sportradarId);
                if (!isSportradarIdParsed)
                    throw new InvalidOperationException("Sportradar ID is not in the correct format.");

                var isDateParsed = DateTime.TryParse(srSportEvent.Date, out DateTime date);
                if (!isDateParsed)
                    throw new InvalidOperationException("Date is not in the correct format.");

                var stageId = dbStages.FirstOrDefault(stage =>
                    stage.StageOrder == srSportEvent.StageId)?.Id ?? 0;
                if (stageId == 0)
                    throw new InvalidOperationException("Stage ID is not found.");

                var homeTeamId = dbTeams.FirstOrDefault(team =>
                    _competitorPrefix + team.SportradarId == srSportEvent.HomeCompetitorId)?.Id ?? 0;
                if (homeTeamId == 0)
                    throw new InvalidOperationException("Home team ID is not found.");

                var awayTeamId = dbTeams.FirstOrDefault(team =>
                    _competitorPrefix + team.SportradarId == srSportEvent.AwayCompetitorId)?.Id ?? 0;
                if (awayTeamId == 0)
                    throw new InvalidOperationException("Away team ID is not found.");

                var venueId = dbVenues.FirstOrDefault(venue =>
                    _venuePrefix + venue.SportradarId == srSportEvent.VenueId)?.Id ?? 0;
                if (venueId == 0)
                    throw new InvalidOperationException("Venue ID is not found.");

                var refereeId = dbReferees.FirstOrDefault(referee =>
                    _refereePrefix + referee.SportradarId == srSportEvent.RefereeId)?.Id ?? 0;
                if (refereeId == 0)
                    throw new InvalidOperationException("Referee ID is not found.");

                matches.Add(new Match
                {
                    StageId = stageId,
                    HomeTeamId = homeTeamId,
                    AwayTeamId = awayTeamId,
                    Date = date,
                    VenueId = venueId,
                    RefereeId = refereeId,
                    HomeTeamScore = srSportEvent.HomeCompetitorScore,
                    AwayTeamScore = srSportEvent.AwayCompetitorScore,
                    SportradarId = sportradarId
                });
            }
        }
        await _context.Matches.AddRangeAsync(matches);
        await _context.SaveChangesAsync();
    }

    private async Task SyncPlayersAsync(List<Team> dbTeams)
    {
        var srPlayers = await _sportradarApiClient.GetPlayersAsync();
        var dbPlayers = await _context.Players.ToListAsync();

        var players = new List<Player>();
        foreach (var srPlayer in srPlayers)
        {
            var dbPlayer = dbPlayers.FirstOrDefault(player => _playerPrefix + player.SportradarId == srPlayer.Id);

            if (dbPlayer == null)
            {
                var isDateParsed = DateOnly.TryParse(srPlayer.DateOfBirth, out DateOnly dateOfBirth);
                if (!isDateParsed)
                    throw new InvalidOperationException("Date of birth is not in the correct format.");

                var teamId = dbTeams.FirstOrDefault(team =>
                    _competitorPrefix + team.SportradarId == srPlayer.CompetitorId)?.Id ?? 0;

                var isSportradarIdParsed = int.TryParse(srPlayer.Id.ToLower().Replace(_playerPrefix, ""), out int sportradarId);
                if (!isSportradarIdParsed)
                    throw new InvalidOperationException("Sportradar ID is not in the correct format.");

                players.Add(new Player
                {
                    Name = srPlayer.Name,
                    Number = srPlayer.JerseyNumber,
                    Position = srPlayer.Type,
                    Height = srPlayer.Height,
                    Nationality = srPlayer.Nationality,
                    DateOfBirth = dateOfBirth,
                    TeamId = teamId,
                    SportradarId = sportradarId
                });
            }
        }
        await _context.Players.AddRangeAsync(players);
        await _context.SaveChangesAsync();
    }

    private async Task SyncStatsAsync(List<Player> dbPlayers, List<Team> dbTeams, List<Match> dbMatches)
    {
        var srSportEvents = await _sportradarApiClient.GetSportEventsAsync();

        var lastTenMatchesSportradarIds = srSportEvents
            .Where(se => DateTime.Parse(se.Date) < DateTime.Now)
            .OrderByDescending(se => se.Date)
            .Select(se => se.Id)
            .Take(10).ToList();

        var teamStats = new List<TeamStat>();
        var playerStats = new List<PlayerStat>();
        foreach (var sportEventId in lastTenMatchesSportradarIds)
        {
            var srCompetitorStats = await _sportradarApiClient.GetCompetitorStatsAsync(sportEventId);
            var dbTeamStats = await _context.TeamStats.ToListAsync();

            foreach (var srCompetitorStat in srCompetitorStats)
            {
                var dbTeamStat = dbTeamStats.FirstOrDefault(ts =>
                    ts.TeamId == dbTeams.FirstOrDefault(team =>
                        _competitorPrefix + team.SportradarId == srCompetitorStat.TeamId)?.Id
                    && ts.MatchId == dbMatches.FirstOrDefault(match =>
                        _sportEventPrefix + match.SportradarId == sportEventId)?.Id);

                if (dbTeamStat == null)
                {
                    var matchId = dbMatches.FirstOrDefault(match =>
                        _sportEventPrefix + match.SportradarId == sportEventId)?.Id ?? 0;
                    if (matchId == 0)
                        throw new InvalidOperationException("Match ID is not found.");

                    var teamId = dbTeams.FirstOrDefault(team =>
                        _competitorPrefix + team.SportradarId == srCompetitorStat.TeamId)?.Id ?? 0;
                    if (teamId == 0)
                        throw new InvalidOperationException("Team ID is not found.");

                    teamStats.Add(new TeamStat
                    {
                        MatchId = matchId,
                        TeamId = teamId,
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
                            _playerPrefix + player.SportradarId == srPlayerStat.PlayerId)?.Id
                        && ps.MatchId == dbMatches.FirstOrDefault(match =>
                            _sportEventPrefix + match.SportradarId == sportEventId)?.Id);

                    if (dbPlayerStat == null)
                    {
                        var matchId = dbMatches.FirstOrDefault(match =>
                            _sportEventPrefix + match.SportradarId == sportEventId)?.Id ?? 0;
                        if (matchId == 0)
                            throw new InvalidOperationException("Match ID is not found.");

                        var playerId = dbPlayers.FirstOrDefault(player =>
                            _playerPrefix + player.SportradarId == srPlayerStat.PlayerId)?.Id ?? 0;
                        if (playerId == 0)
                            throw new InvalidOperationException("Player ID is not found.");

                        var teamId = dbTeams.FirstOrDefault(team =>
                            _competitorPrefix + team.SportradarId == srPlayerStat.CompetitorId)?.Id ?? 0;

                        playerStats.Add(new PlayerStat
                        {
                            MatchId = matchId,
                            PlayerId = playerId,
                            TeamId = teamId,
                            Goals = srPlayerStat.GoalsScored,
                            Assists = srPlayerStat.Assists,
                            Shots = srPlayerStat.ShotsTotal,
                            ShotsOnTarget = srPlayerStat.ShotsOnTarget,
                            YellowCards = srPlayerStat.YellowCards,
                            RedCards = srPlayerStat.RedCards,
                        });
                    }
                }
            }
        }
        await _context.TeamStats.AddRangeAsync(teamStats);
        await _context.PlayerStats.AddRangeAsync(playerStats);
        await _context.SaveChangesAsync();
    }

    private async Task SyncStandingsAsync(List<Team> dbTeams, List<Stage> dbStages)
    {
        var srStandings = await _sportradarApiClient.GetStandingsAsync();
        var dbStandings = await _context.Standings.ToListAsync();

        var standings = new List<Standing>();
        foreach (var srStanding in srStandings)
        {
            var dbStanding = dbStandings.FirstOrDefault(standing =>
                standing.TeamId == dbTeams.FirstOrDefault(team =>
                    _competitorPrefix + team.SportradarId == srStanding.CompetitorId)?.Id);

            if (dbStanding == null)
            {
                var teamId = dbTeams.FirstOrDefault(team =>
                    _competitorPrefix + team.SportradarId == srStanding.CompetitorId)?.Id ?? 0;
                if (teamId == 0)
                    throw new InvalidOperationException("Team ID is not found.");

                var stageId = dbStages.FirstOrDefault(stage =>
                    srStanding.StageId == stage.StageOrder)?.Id ?? 0;
                if (stageId == 0)
                    throw new InvalidOperationException("Stage ID is not found.");

                standings.Add(new Standing
                {
                    TeamId = teamId,
                    StageId = stageId,
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
        await _context.Standings.AddRangeAsync(standings);
        await _context.SaveChangesAsync();
    }
}