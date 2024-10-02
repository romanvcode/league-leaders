using LeagueLeaders.Infrastructure.Clients.SportradarApi;
using Microsoft.Extensions.Options;
using FluentAssertions;
using Moq;
using Moq.Protected;
using System.Net;
using FluentAssertions.Execution;

namespace LeagueLeaders.Tests.IntegrationTests;

public class SportradarApiClientTest
{
    private readonly IOptions<SportradarSettings> _options;
    private readonly ISportradarApiClient _sportradarApiClient;
    private readonly HttpClient _httpClient;
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;

    public SportradarApiClientTest()
    {
        _options = Options.Create(new SportradarSettings
        {
            ApiKey = "test-api-key",
            ChampionsLeague = "test-league",
            CurrentSeason = "test-season"
        });

        _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("https://api.sportradar.com")
        };

        _sportradarApiClient = new SportradarApiClient(_httpClient, _options);
    }
}