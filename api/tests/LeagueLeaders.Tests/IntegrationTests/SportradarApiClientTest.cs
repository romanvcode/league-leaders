using FluentAssertions;
using FluentAssertions.Execution;
using LeagueLeaders.Infrastructure.Clients.SportradarApi;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;

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
            ApiKey = "api-key",
            ChampionsLeague = "champions-league",
            CurrentSeason = "current-season"
        });

        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("https://api.sportradar.com")
        };

        _sportradarApiClient = new SportradarApiClient(_httpClient, _options);
    }

    [Fact]
    public async Task GetCompetitionAsync_ShouldReturnCompetition()
    {
        var expectedResponse = @"
        {
            ""competition"": {
                ""id"": ""champions-league"",
                ""name"": ""Champions League"",
                ""category"": { 
                    ""name"": ""Europe"" 
                }
            }
        }";

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedResponse)
            });

        var result = await _sportradarApiClient.GetCompetitionAsync();

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Id.Should().Be("champions-league");
            result.Name.Should().Be("Champions League");
            result.Region.Should().Be("Europe");
        }
    }

    [Fact]
    public async Task GetSeasonsAsync_ShouldReturnSeasons()
    {
        var expectedResponse = @"
        {
            ""seasons"": [
                {
                    ""id"": ""season"",
                    ""name"": ""2024/2025"",
                    ""start_date"": ""2024-08-01"",
                    ""end_date"": ""2025-05-31"",
                    ""competition_id"": ""champions-league""
                }
            ]
        }";

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedResponse)
            });

        var result = await _sportradarApiClient.GetSeasonsAsync();

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Id.Should().Be("season");
            result.First().Name.Should().Be("2024/2025");
            result.First().StartDate.Should().Be("2024-08-01");
            result.First().EndDate.Should().Be("2025-05-31");
            result.First().CompetitionId.Should().Be("champions-league");
        }
    }

    [Fact]
    public async Task GetStagesAsync_ShouldReturnStages()
    {
        var expectedResponse = @"
        {
            ""stages"": [
                {
                    ""order"": 1,
                    ""phase"": ""playoff"",
                    ""type"": ""knockout""
                }
            ]
        }";

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedResponse)
            });

        var result = await _sportradarApiClient.GetStagesAsync();

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Order.Should().Be(1);
            result.First().Phase.Should().Be("playoff");
            result.First().Type.Should().Be("knockout");
            result.First().SeasonId.Should().Be("current-season");
        }
    }

    [Fact]
    public async Task GetSportEventsAsync_ShouldReturnSportEvents()
    {
        var expectedResponse = @"
        {
            ""summaries"": [
                {
                    ""sport_event"": {
                        ""id"": ""sport_event"",
                        ""sport_event_context"": {
                            ""stage"": {
                                ""order"": 1
                            }
                        },
                        ""competitors"": [
                            {
                                ""id"": ""home_team"",
                                ""qualifier"": ""home"" 
                            },
                            {
                                ""id"": ""away_team"",
                                ""qualifier"": ""away""
                            }
                        ],
                        ""start_time"": ""2024-08-01"",
                        ""venue"": {
                            ""id"": ""venue""
                        },
                        ""sport_event_conditions"": {
                            ""referees"": [
                                {
                                    ""id"": ""referee"",
                                    ""type"": ""main_referee""
                                }
                            ]
                        }
                    },
                    ""sport_event_status"": {
                        ""home_score"": 2,
                        ""away_score"": 1
                    }   
                }
            ]
        }";

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedResponse)
            });

        var result = await _sportradarApiClient.GetSportEventsAsync();

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Id.Should().Be("sport_event");
            result.First().StageId.Should().Be(1);
            result.First().HomeCompetitorId.Should().Be("home_team");
            result.First().AwayCompetitorId.Should().Be("away_team");
            result.First().Date.Should().Be("2024-08-01");
            result.First().VenueId.Should().Be("venue");
            result.First().RefereeId.Should().Be("referee");
            result.First().HomeCompetitorScore.Should().Be(2);
            result.First().AwayCompetitorScore.Should().Be(1);
        }
    }

    [Fact]
    public async Task GetCompetitorsAsync_ShouldReturnCompetitors()
    {
        var expectedResponse = @"
        {
            ""season_competitors"": [
                {
                    ""id"": ""real-madrid"",
                    ""name"": ""Real Madrid"",
                    ""abbreviation"": ""RMA""
                }
            ],
            ""competitor"": {
                ""id"": ""real-madrid"",
                ""country"": ""Spain"",
                ""qualifier"": ""team""
            },
            ""manager"": {
                ""name"": ""Carlo Ancelotti""
            },
            ""venue"": {
                ""id"": ""santiago-barnabeu"",
                ""name"": ""Santiago Barnabeu"",
                ""city_name"": ""Madrid"",
                ""country_name"": ""Spain"",
                ""capacity"": 81044
            }
        }";

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedResponse)
            });

        var result = await _sportradarApiClient.GetCompetitorsAsync();

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Id.Should().Be("real-madrid");
            result.First().Name.Should().Be("Real Madrid");
            result.First().Abbreviation.Should().Be("RMA");
            result.First().Country.Should().Be("Spain");
            result.First().Stadium.Should().Be("Santiago Barnabeu");
            result.First().Manager.Should().Be("Carlo Ancelotti");
        }
    }

    [Fact]
    public async Task GetPlayersAsync_ShouldReturnPlayers()
    {
        var expectedResponse = @"
        {
            ""season_competitor_players"": [
                {
                    ""id"": ""real-madrid"",    
                    ""players"": [
                        {
                            ""id"": ""cr7"",
                            ""name"": ""Crisiano Ronaldo"",
                            ""type"": ""forward"",
                            ""jersey_number"": 7,
                            ""height"": 187,
                            ""nationality"": ""Portugal"",
                            ""date_of_birth"": ""1985-02-05""
                        }
                    ]
                }
            ]
        }";

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedResponse)
            });

        var result = await _sportradarApiClient.GetPlayersAsync();

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Id.Should().Be("cr7");
            result.First().CompetitorId.Should().Be("real-madrid");
            result.First().Name.Should().Be("Crisiano Ronaldo");
            result.First().Type.Should().Be("forward");
            result.First().JerseyNumber.Should().Be(7);
            result.First().Height.Should().Be(187);
            result.First().Nationality.Should().Be("Portugal");
            result.First().DateOfBirth.Should().Be("1985-02-05");
        }
    }

    [Fact]
    public async Task GetRefereesAsync_ShouldReturnReferees()
    {
        var expectedResponse = @"
        {
            ""summaries"": [
                {
                    ""sport_event"": {
                        ""sport_event_conditions"": {
                            ""referees"": [
                                {
                                    ""id"": ""referee"",
                                    ""name"": ""Pierluigi Collina"",        
                                    ""nationality"": ""Italy"",
                                    ""type"": ""main_referee""
                                }
                            ]
                        }
                    }
                }
            ]
        }";

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedResponse)
            });

        var result = await _sportradarApiClient.GetRefereesAsync();

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Id.Should().Be("referee");
            result.First().Name.Should().Be("Pierluigi Collina");
            result.First().Nationality.Should().Be("Italy");
        }
    }

    [Fact]
    public async Task GetVenuesAsync_ShouldReturnVenues()
    {
        var expectedResponse = @"
        {
            ""summaries"": [
                {
                    ""sport_event"": {
                        ""venue"": {
                            ""id"": ""santiago-barnabeu"",
                            ""name"": ""Santiago Barnabeu"",
                            ""city_name"": ""Madrid"",
                            ""country_name"": ""Spain"",
                            ""capacity"": 81044
                        }
                    }
                }
            ]
        }";

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedResponse)
            });

        var result = await _sportradarApiClient.GetVenuesAsync();

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Id.Should().Be("santiago-barnabeu");
            result.First().Name.Should().Be("Santiago Barnabeu");
            result.First().CityName.Should().Be("Madrid");
            result.First().CountryName.Should().Be("Spain");
            result.First().Capacity.Should().Be(81044);
        }
    }

    [Fact]
    public async Task GetCompetitorStatsAsync_ShouldReturnCompetitorStats()
    {
        var expectedResponse = @"
        {
            ""statistics"": {
                ""totals"": {
                    ""competitors"": [
                        {
                            ""id"": ""real-madrid"",
                            ""statistics"": {
                                ""ball_possession"": 55,
                                ""red_cards"": 0,
                                ""yellow_cards"": 3,
                                ""corner_kicks"": 5,
                                ""offsides"": 3,
                                ""fouls"": 2,
                                ""shots_total"": 5,
                                ""shots_on_target"": 3   
                            }      
                        }
                    ]
                }
            }
        }";

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedResponse)
            });

        var result = await _sportradarApiClient.GetCompetitorStatsAsync("sport_event");

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().TeamId.Should().Be("real-madrid");
            result.First().Possession.Should().Be(55);
            result.First().RedCards.Should().Be(0);
            result.First().YellowCards.Should().Be(3);
            result.First().CornerKicks.Should().Be(5);
            result.First().Offsides.Should().Be(3);
            result.First().Fouls.Should().Be(2);
            result.First().ShotsTotal.Should().Be(5);
            result.First().ShotsOnTarget.Should().Be(3);
        }
    }

    [Fact]
    public async Task GetPlayerStatsAsync_ShouldReturnPlayerStats()
    {
        var expectedResponse = @"
        {
            ""statistics"": {
                ""totals"": {
                    ""competitors"": [
                        {
                            ""id"": ""real-madrid"",
                            ""players"": [
                                {
                                    ""id"": ""cr7"",
                                    ""statistics"": {
                                        ""goals_scored"": 3,
                                        ""assists"": 1,
                                        ""red_cards"": 0,
                                        ""yellow_cards"": 0,
                                        ""offsides"": 3,
                                        ""fouls"": 2,
                                        ""shots_on_target"": 5,
                                        ""shots_off_target"": 3   
                                    }      
                                }
                            ]
                        }
                    ]
                }
            }
        }";

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedResponse)
            });

        var result = await _sportradarApiClient.GetPlayerStatsAsync("sport_event");

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().PlayerId.Should().Be("cr7");
            result.First().CompetitorId.Should().Be("real-madrid");
            result.First().GoalsScored.Should().Be(3);
            result.First().Assists.Should().Be(1);
            result.First().RedCards.Should().Be(0);
            result.First().YellowCards.Should().Be(0);
            result.First().ShotsTotal.Should().Be(8);
            result.First().ShotsOnTarget.Should().Be(5);
        }
    }

    [Fact]
    public async Task GetStandingsAsync_ShouldReturnStandings()
    {
        var expectedResponse = @"
        {
            ""standings"": [
                {
                    ""groups"": [
                        {
                            ""stage"": {
                                ""order"": 1 
                            },
                            ""standings"": [
                                {
                                    ""competitor"": {
                                        ""id"": ""real-madrid""
                                    },
                                    ""points"": 9,
                                    ""rank"": 1,
                                    ""played"": 3,
                                    ""win"": 3,
                                    ""draw"": 0,
                                    ""loss"": 0,
                                    ""goals_for"": 8,
                                    ""goals_against"": 2
                                }
                            ]
                        }
                    ]
                }
            ]
        }";

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedResponse)
            });

        var result = await _sportradarApiClient.GetStandingsAsync();

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().StageId.Should().Be(1);
            result.First().CompetitorId.Should().Be("real-madrid");
            result.First().Points.Should().Be(9);
            result.First().Rank.Should().Be(1);
            result.First().Played.Should().Be(3);
            result.First().Win.Should().Be(3);
            result.First().Draw.Should().Be(0);
            result.First().Loss.Should().Be(0);
            result.First().GoalsFor.Should().Be(8);
            result.First().GoalsAgainst.Should().Be(2);
        }
    }
}