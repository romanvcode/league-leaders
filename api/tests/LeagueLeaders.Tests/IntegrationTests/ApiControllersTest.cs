using FluentAssertions;
using LeagueLeaders.Domain;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;

namespace LeagueLeaders.Tests.IntegrationTests
{
    public class ApiControllersTest : IClassFixture<LeagueLeadersWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ApiControllersTest(LeagueLeadersWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetStandings_ToBeSuccess()
        {
            HttpResponseMessage response = await
                _client.GetAsync("/api/leaderboard/standings");

            response.EnsureSuccessStatusCode();

            var expectedStandings = new List<Standing>
            {
                new Standing { Id = 1, Place = 1, TeamId = 1, StageId = 1 },
                new Standing { Id = 2, Place = 2, TeamId = 2, StageId = 1 },
            };

            var content = await response.Content.ReadAsStringAsync();
            var actualStandings = JsonConvert.DeserializeObject<List<Standing>>(content);

            actualStandings.Should().BeEquivalentTo(expectedStandings,
                options => options.Including(s => s.Id).Including(s => s.Place)
                .Including(s => s.TeamId).Including(s => s.StageId));
        }

        [Fact]
        public async Task GetScheduleMatches_ToBeSuccess()
        {
            HttpResponseMessage response = await
                _client.GetAsync("/api/schedule/matches");

            response.EnsureSuccessStatusCode();

            var expectedMatches = new List<Match>
            {
                new Match { Id = 1, HomeTeamId = 1, AwayTeamId = 2, StageId = 1 }
            };

            var content = await response.Content.ReadAsStringAsync();
            var actualMatches = JsonConvert.DeserializeObject<List<Match>>(content);

            actualMatches.Should().BeEquivalentTo(expectedMatches,
                options => options.Including(m => m.Id).Including(m => m.HomeTeamId)
                .Including(m => m.AwayTeamId).Including(m => m.StageId));
        }

        [Fact]
        public async Task GetTeamById_ToBeSuccess()
        {
            HttpResponseMessage response = await
                _client.GetAsync("/api/teams/1");

            response.EnsureSuccessStatusCode();

            var expectedTeam = new Team
            {
                Id = 1,
                Name = "Team A",
                Abbreviation = "TA",
                Country = "Country A"
            };

            var content = await response.Content.ReadAsStringAsync();
            var actualTeam = JsonConvert.DeserializeObject<Team>(content);

            actualTeam.Should().BeEquivalentTo(expectedTeam,
                options => options.Including(t => t.Id).Including(t => t.Name)
                .Including(t => t.Abbreviation).Including(t => t.Country));
        }

        [Fact]
        public async Task GetTeamById_ToBeFailure()
        {
            HttpResponseMessage response = await
                _client.GetAsync("/api/teams/100");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetTeamPlayersAsync_ToBeSuccess()
        {
            HttpResponseMessage response = await
                _client.GetAsync("/api/teams/1/players");

            response.EnsureSuccessStatusCode();

            var expectedPlayers = new List<Player>
            {
                new Player { Id = 1, Name = "Player A", TeamId = 1 }
            };

            var content = await response.Content.ReadAsStringAsync();
            var actualPlayers = JsonConvert.DeserializeObject<List<Player>>(content);

            actualPlayers.Should().BeEquivalentTo(expectedPlayers,
                options => options.Including(p => p.Id).Including(p => p.Name)
                .Including(p => p.TeamId));
        }

        [Fact]
        public async Task GetTeamPlayersAsync_ToBeFailure()
        {
            HttpResponseMessage response = await
                _client.GetAsync("/api/teams/100/players");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetLastFiveTeamMatchesAsync_ToBeSuccess()
        {
            HttpResponseMessage response = await
                _client.GetAsync("/api/teams/1/matches");

            response.EnsureSuccessStatusCode();

            var expectedMatches = new List<Match>
            {
                new Match { Id = 2, HomeTeamId = 2, AwayTeamId = 1, StageId = 1 }
            };

            var content = await response.Content.ReadAsStringAsync();
            var actualMatches = JsonConvert.DeserializeObject<List<Match>>(content);

            actualMatches.Should().BeEquivalentTo(expectedMatches,
                options => options.Including(m => m.Id).Including(m => m.HomeTeamId)
                .Including(m => m.AwayTeamId).Including(m => m.StageId));
        }

        [Fact]
        public async Task GetLastFiveTeamMatchesAsync_ToBeFailure()
        {
            HttpResponseMessage response = await
                _client.GetAsync("/api/teams/100/matches");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetTeamsBySearchTerm_ToBeSuccess()
        {
            HttpResponseMessage response = await
                _client.GetAsync("/api/teams/search/Team");

            response.EnsureSuccessStatusCode();

            var expectedTeams = new List<Team>
            {
                new Team { Id = 1, Name = "Team A", Abbreviation = "TA", Country = "Country A" },
                new Team { Id = 2, Name = "Team B", Abbreviation = "TB", Country = "Country B" }
            };

            var content = await response.Content.ReadAsStringAsync();
            var actualTeams = JsonConvert.DeserializeObject<List<Team>>(content);

            actualTeams.Should().BeEquivalentTo(expectedTeams,
                options => options.Including(t => t.Id).Including(t => t.Name)
                .Including(t => t.Abbreviation).Including(t => t.Country));
        }

        [Fact]
        public async Task GetTeamsBySearchTerm_ToBeFailure()
        {
            HttpResponseMessage response = await
                _client.GetAsync("/api/teams/search/");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
