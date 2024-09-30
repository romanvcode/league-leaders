using LeagueLeaders.Infrastructure.Clients.SportradarApi.Exceptions;
using LeagueLeaders.Infrastructure.Clients.SportradarApi.Models;
using LeagueLeaders.Infrastructure.Clients.SportradarApi.Responses;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace LeagueLeaders.Infrastructure.Clients.SportradarApi;

public class SportradarApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _championsLeague;
    private readonly string _currentSeason;
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
    };

    public SportradarApiClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["Sportradar:ApiKey"] ?? "";
        _championsLeague = configuration["Sportradar:Competitions:ChampionsLeague"] ?? "";
        _currentSeason = configuration["Sportradar:Seasons:Current"] ?? "";
    }

    public async Task<Models.Competition> GetCompetitionAsync()
    {
        var competitionResponse = await GetResponse<CompetitionResponse>($"competitions/{_championsLeague}/info?api_key={_apiKey}") 
            ?? throw new DeserializationException($"Failed to deserialize competition: {_championsLeague}.");
        
        var competition = new Models.Competition
        {
            Id = competitionResponse.Competition.Id,
            Name = competitionResponse.Competition.Name,
            Region = competitionResponse.Competition.Category.Name
        };

        return competition;
    }

    public async Task<List<Season>> GetSeasonsAsync()
    {
        var seasonResponse = await GetResponse<SeasonsResponse>($"competitions/{_championsLeague}/seasons?api_key={_apiKey}") 
            ?? throw new DeserializationException($"Failed to deserialize seasons for the {_championsLeague} competition.");
        
        var seasons = seasonResponse.Seasons;

        return seasons;
    }

    public async Task<List<Stage>> GetStagesAsync()
    {
        var stageResponse = await GetResponse<StagesResponse>($"seasons/{_currentSeason}/stages_groups_cup_rounds?api_key={_apiKey}") 
            ?? throw new DeserializationException($"Failed to deserialize stages for the {_currentSeason}.");
        
        var stages = stageResponse.Stages;

        foreach (var stage in stages)
        {
            stage.SeasonId = _currentSeason;
        }

        return stages;
    }

    public async Task<List<Models.Competitor>> GetCompetitorsAsync()
    {
        var competitorsResponse = await GetResponse<CompetitorsResponse>($"seasons/{_currentSeason}/competitors?api_key={_apiKey}") 
            ?? throw new DeserializationException($"Failed to deserialize competitors for the {_currentSeason}.");
        
        var competitors = competitorsResponse.SeasonCompetitors;

        foreach (var competitor in competitors)
        {
            var profileResponse = await GetResponse<CompetitorProfileResponse>($"seasons/{_currentSeason}/competitors?api_key={_apiKey}");

            if (profileResponse == null)
            {
                continue;
            }

            competitor.Country = profileResponse.Competitor.Country;
            competitor.Stadium = profileResponse.Venue.Name;
            competitor.Manager = profileResponse.Manager.Name;
        }

        return competitors;
    }

    private async Task<T?> GetResponse<T>(string url)
    {
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, _options);
    }
}