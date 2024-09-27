using System.Text.Json;

namespace LeagueLeaders.Infrastructure.HttpClients;
public class SoccerApiClient
{
    private readonly HttpClient _httpClient;

    public SoccerApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Competition>> GetCompetitionsAsync(string endpoint)
    {
        var response = await _httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();

        var jsonDocument = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var rootElement = jsonDocument.RootElement.GetProperty("competitions").ToString();
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        };

        var result = JsonSerializer.Deserialize<List<Competition>>(rootElement, options);

        if (result == null)
        {
            throw new Exception("Failed to deserialize response from the API.");
        }

        return result;
    }
}
