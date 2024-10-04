using LeagueLeaders.Infrastructure.Clients.SportradarApi.Exceptions;
using LeagueLeaders.Infrastructure.Clients.SportradarApi.Models;
using LeagueLeaders.Infrastructure.Clients.SportradarApi.Responses;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace LeagueLeaders.Infrastructure.Clients.SportradarApi;

public class SportradarApiClient : ISportradarApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _championsLeague;
    private readonly string _currentSeason;
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
    };

    public SportradarApiClient(HttpClient httpClient, IOptions<SportradarSettings> settings)
    {
        _httpClient = httpClient;
        _apiKey = settings.Value.ApiKey
            ?? throw new SportradarSettingsNullException(nameof(settings.Value.ApiKey));
        _championsLeague = settings.Value.ChampionsLeague
            ?? throw new SportradarSettingsNullException(nameof(settings.Value.ChampionsLeague));
        _currentSeason = settings.Value.CurrentSeason
            ?? throw new SportradarSettingsNullException(nameof(settings.Value.CurrentSeason));
    }

    public async Task<Competition> GetCompetitionAsync()
    {
        var url = $"competitions/{_championsLeague}/info?api_key={_apiKey}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var competitionResponse = JsonSerializer.Deserialize<CompetitionsResponse>(content, _options)
            ?? throw new SportradarBadResponseException($"Failed to deserialize competition response: {content}.");

        return new Competition
        {
            Id = competitionResponse.Competition.Id,
            Name = competitionResponse.Competition.Name,
            Region = competitionResponse.Competition.Category.Name
        };
    }

    public async Task<List<Season>> GetSeasonsAsync()
    {
        var url = $"competitions/{_championsLeague}/seasons?api_key={_apiKey}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var seasonResponse = JsonSerializer.Deserialize<SeasonsResponse>(content, _options)
            ?? throw new SportradarBadResponseException($"Failed to deserialize seasons response: {content}.");

        return seasonResponse.Seasons;
    }

    public async Task<List<Stage>> GetStagesAsync()
    {
        var url = $"seasons/{_currentSeason}/stages_groups_cup_rounds?api_key={_apiKey}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var stageResponse = JsonSerializer.Deserialize<StagesResponse>(content, _options)
            ?? throw new SportradarBadResponseException($"Failed to deserialize stages response: {content}.");

        var stages = stageResponse.Stages;
        foreach (var stage in stages)
        {
            stage.SeasonId = _currentSeason;
        }

        return stages;
    }

    public async Task<List<Competitor>> GetCompetitorsAsync()
    {
        var url = $"seasons/{_currentSeason}/competitors?api_key={_apiKey}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var competitorsResponse = JsonSerializer.Deserialize<CompetitorsResponse>(content, _options)
            ?? throw new SportradarBadResponseException($"Failed to deserialize competitors response: {content}.");

        var competitors = competitorsResponse.SeasonCompetitors;
        foreach (var competitor in competitors)
        {
            url = $"competitors/{competitor.Id}/profile?api_key={_apiKey}";

            var profileResponse = await _httpClient.GetAsync(url);
            profileResponse.EnsureSuccessStatusCode();

            var profileContent = await profileResponse.Content.ReadAsStringAsync();
            var profileData = JsonSerializer.Deserialize<CompetitorProfileResponse>(profileContent, _options)
                ?? throw new SportradarBadResponseException($"Failed to deserialize profile response for competitor {competitor.Id}: {profileContent}.");

            competitor.Country = profileData.Competitor.Country;
            competitor.Stadium = profileData.Venue.Name;
            competitor.Manager = profileData.Manager.Name;
        }

        return competitors;
    }

    public async Task<List<SportEvent>> GetSportEventsAsync()
    {
        var url = $"seasons/{_currentSeason}/summaries?api_key={_apiKey}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var sportEventsResponse = JsonSerializer.Deserialize<SportEventsResponse>(content, _options)
            ?? throw new SportradarBadResponseException($"Failed to deserialize sport events response: {content}.");

        var sportEvents = sportEventsResponse.Summaries;
        var events = new List<SportEvent>();

        foreach (var sportEvent in sportEvents)
        {
            if (sportEvent.SportEvent.SportEventConditions.Referees == null)
            {
                continue;
            }

            events.Add(new SportEvent
            {
                Id = sportEvent.SportEvent.Id,
                StageId = sportEvent.SportEvent.SportEventContext.Stage.Order,
                HomeCompetitorId = sportEvent.SportEvent.Competitors.Single(c => c.Qualifier == "home").Id,
                AwayCompetitorId = sportEvent.SportEvent.Competitors.Single(c => c.Qualifier == "away").Id,
                Date = sportEvent.SportEvent.StartTime,
                VenueId = sportEvent.SportEvent.Venue.Id,
                RefereeId = sportEvent.SportEvent.SportEventConditions.Referees.Single(r => r.Type == "main_referee").Id,
                HomeCompetitorScore = sportEvent.SportEventStatus.HomeScore,
                AwayCompetitorScore = sportEvent.SportEventStatus.AwayScore
            });
        }

        return events;
    }

    public async Task<List<Player>> GetPlayersAsync()
    {
        var url = $"seasons/{_currentSeason}/competitor_players?api_key={_apiKey}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var playersResponse = JsonSerializer.Deserialize<PlayersResponse>(content, _options)
            ?? throw new SportradarBadResponseException($"Failed to deserialize players response: {content}.");

        var competitorPlayers = playersResponse.SeasonCompetitorPlayers;
        var players = new List<Player>();

        foreach (var competitorPlayer in competitorPlayers)
        {
            if (competitorPlayer.Players == null)
            {
                continue;
            }

            players.AddRange(competitorPlayer.Players.Select(p => new Player
            {
                Id = p.Id,
                CompetitorId = competitorPlayer.Id,
                Name = p.Name,
                Type = p.Type,
                JerseyNumber = p.JerseyNumber,
                Height = p.Height,
                Nationality = p.Nationality,
                DateOfBirth = p.DateOfBirth
            }));
        }

        return players;
    }

    public async Task<List<Venue>> GetVenuesAsync()
    {
        var url = $"seasons/{_currentSeason}/summaries?api_key={_apiKey}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var venuesResponse = JsonSerializer.Deserialize<VenuesResponse>(content, _options)
            ?? throw new SportradarBadResponseException($"Failed to deserialize venues response: {content}.");

        var venues = new List<Venue>();
        foreach (var venue in venuesResponse.Summaries.Select(s => s.SportEvent.Venue))
        {
            venues.Add(new Venue
            {
                Id = venue.Id,
                Name = venue.Name,
                CityName = venue.CityName,
                CountryName = venue.CountryName,
                Capacity = venue.Capacity
            });
        }

        return venues;
    }

    public async Task<List<Referee>> GetRefereesAsync()
    {
        var url = $"seasons/{_currentSeason}/summaries?api_key={_apiKey}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var refereesResponse = JsonSerializer.Deserialize<RefereesResponse>(content, _options)
            ?? throw new SportradarBadResponseException($"Failed to deserialize referees response: {content}.");

        var referees = new List<Referee>();
        foreach (var se in refereesResponse.Summaries.Select(s => s.SportEvent))
        {
            if (se.SportEventConditions == null || se.SportEventConditions.Referees == null)
            {
                continue;
            }

            var referee = se.SportEventConditions.Referees.Single(r => r.Type == "main_referee");
            referees.Add(new Referee
            {
                Id = referee.Id,
                Name = referee.Name,
                Nationality = referee.Nationality
            });
        }

        return referees;
    }

    public async Task<List<CompetitorStats>> GetCompetitorStatsAsync(string sportEventId)
    {
        var response = await _httpClient.GetAsync($"sport_events/{sportEventId}/summary?api_key={_apiKey}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var statsResponse = JsonSerializer.Deserialize<CompetitorsStatsResponse>(content, _options)
            ?? throw new SportradarBadResponseException($"Failed to deserialize competitor stats response: {content}.");

        var stats = new List<CompetitorStats>();
        foreach (var competitor in statsResponse.Statistics.Totals.Competitors)
        {
            stats.Add(new CompetitorStats
            {
                SportEventId = sportEventId,
                TeamId = competitor.Id,
                Possession = competitor.Statistics.BallPossession,
                RedCards = competitor.Statistics.RedCards,
                YellowCards = competitor.Statistics.YellowCards,
                CornerKicks = competitor.Statistics.CornerKicks,
                Offsides = competitor.Statistics.Offsides,
                Fouls = competitor.Statistics.Fouls,
                ShotsTotal = competitor.Statistics.ShotsTotal,
                ShotsOnTarget = competitor.Statistics.ShotsOnTarget
            });
        }

        return stats;
    }

    public async Task<List<PlayerStats>> GetPlayerStatsAsync(string sportEventId)
    {
        var url = $"sport_events/{sportEventId}/summary?api_key={_apiKey}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var statsResponse = JsonSerializer.Deserialize<PlayersStatsResponse>(content, _options)
            ?? throw new SportradarBadResponseException($"Failed to deserialize player stats response: {content}.");

        var stats = new List<PlayerStats>();
        foreach (var competitor in statsResponse.Statistics.Totals.Competitors)
        {
            var competitorId = competitor.Id;
            foreach (var player in competitor.Players)
            {
                stats.Add(new PlayerStats
                {
                    PlayerId = player.Id,
                    CompetitorId = competitorId,
                    SportEventId = sportEventId,
                    GoalsScored = player.Statistics.GoalsScored,
                    Assists = player.Statistics.Assists,
                    RedCards = player.Statistics.RedCards,
                    YellowCards = player.Statistics.YellowCards,
                    ShotsTotal = player.Statistics.ShotsOffTarget + player.Statistics.ShotsOnTarget,
                    ShotsOnTarget = player.Statistics.ShotsOnTarget,
                });
            }
        }

        return stats;
    }

    public async Task<List<Standing>> GetStandingsAsync()
    {
        var url = $"seasons/{_currentSeason}/standings?api_key={_apiKey}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var standingsResponse = JsonSerializer.Deserialize<StandingsResponse>(content, _options)
            ?? throw new SportradarBadResponseException($"Failed to deserialize standings response: {content}.");

        var standings = new List<Standing>();
        foreach (var standing in standingsResponse.Standings)
        {
            foreach (var group in standing.Groups)
            {
                var stageId = group.Stage.Order;
                foreach (var competitorStanding in group.Standings)
                {
                    standings.Add(new Standing
                    {
                        StageId = stageId,
                        CompetitorId = competitorStanding.Competitor.Id,
                        Points = competitorStanding.Points,
                        Rank = competitorStanding.Rank,
                        Played = competitorStanding.Played,
                        Win = competitorStanding.Win,
                        Draw = competitorStanding.Draw,
                        Loss = competitorStanding.Loss,
                        GoalsFor = competitorStanding.GoalsFor,
                        GoalsAgainst = competitorStanding.GoalsAgainst
                    });
                }
            }
        }

        return standings;
    }
}