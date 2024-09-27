using LeagueLeaders.Infrastructure.HttpClients.Models;
using System.Text.Json;

namespace LeagueLeaders.Infrastructure.HttpClients;
public class SoccerApiClient
{
    private readonly HttpClient _httpClient;

    public SoccerApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // url: competitions/{urn_competition}/info?api_key=api_key
    public async Task<Competition> GetCompetitionsAsync(string endpoint)
    {
        var response = await _httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();

        var jsonDocument = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var rootElement = jsonDocument.RootElement.GetProperty("competition").ToString();
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        };

        var result = JsonSerializer.Deserialize<Competition>(rootElement, options);

        if (result == null)
        {
            throw new Exception("Failed to deserialize response from the API.");
        }

        return result;
    }

    // url: competitions/{urn_competition}/seasons?api_key=api_key
    public async Task<List<Season>> GetSeasonsAsync(string endpoint)
    {
        var response = await _httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();

        var jsonDocument = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var rootElement = jsonDocument.RootElement.GetProperty("seasons").ToString();
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        };

        var result = JsonSerializer.Deserialize<List<Season>>(rootElement, options);

        if (result == null)
        {
            throw new Exception("Failed to deserialize response from the API.");
        }

        return result;
    }

    // url: seasons/{urn_season}/stages_groups_cup_rounds?api_key=api_key
    public async Task<List<Stage>> GetStagesAsync(string endpoint)
    {
        var response = await _httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();

        var jsonDocument = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var rootElement = jsonDocument.RootElement.GetProperty("stages").ToString();
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        };

        var result = JsonSerializer.Deserialize<List<Stage>>(rootElement, options);

        var seasonId = endpoint.Split('/')[1];
        foreach (var stage in result)
        {
            stage.SeasonId = seasonId;
        }

        if (result == null)
        {
            throw new Exception("Failed to deserialize response from the API.");
        }

        return result;
    }

    // url: seasons/{urn_season}/competitors?api_key=api_key
    public async Task<List<Competitor>> GetCompetitorsAsync()
    {
        string endpoint = "seasons/sr:season:119239/competitors?api_key=N8hl4okmaOauAfEmFgkObbsJHUXSpv0uxywwvg5n";

        var response = await _httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();

        var jsonDocument = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var rootElement = jsonDocument.RootElement.GetProperty("season_competitors").ToString();
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        };

        var result = JsonSerializer.Deserialize<List<Competitor>>(rootElement, options);

        if (result == null)
        {
            throw new Exception("Failed to deserialize response from the API.");
        }

        var endpoint2 = "";
        foreach (var competitor in result) {
            endpoint2 = $"competitors/{competitor.Id}/profile?api_key=N8hl4okmaOauAfEmFgkObbsJHUXSpv0uxywwvg5n";
            var response2 = await _httpClient.GetAsync(endpoint2);
            response2.EnsureSuccessStatusCode();

            var jsonDocument2 = JsonDocument.Parse(await response2.Content.ReadAsStringAsync());
            var rootElement2 = jsonDocument2.RootElement.GetProperty("competitor").ToString();

            var competitorProfile = JsonSerializer.Deserialize<Competitor>(rootElement2, options);

            competitor.Country = competitorProfile.Country;

            var rootElement3 = jsonDocument2.RootElement.GetProperty("manager").ToString();

            var competitorManager = JsonSerializer.Deserialize<Manager>(rootElement3, options);

            competitor.Manager = competitorManager.Name;
        }

        return result;
    }
}
