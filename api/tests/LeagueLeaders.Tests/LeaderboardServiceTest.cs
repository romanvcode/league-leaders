using FluentAssertions;
using LeagueLeaders.Application;
using LeagueLeaders.Application.Exceptions;
using LeagueLeaders.Domain;
using LeagueLeaders.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LeagueLeaders.Tests
{
    public class LeaderboardServiceTest : IDisposable
    {
        private readonly LeagueLeadersDbContext _context;
        private readonly LeaderboardService _leaderboardService;
        public LeaderboardServiceTest()
        {
            var options = new DbContextOptionsBuilder<LeagueLeadersDbContext>()
                .UseInMemoryDatabase(databaseName: "LeagueLeadersLeaderboardDB")
                .Options;
            _context = new LeagueLeadersDbContext(options);
            _leaderboardService = new LeaderboardService(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }

        #region GetStandingsForEachTeamAsync
        [Fact]
        public async Task GetStandingsForEachTeamAsync_CurrentSeasonIsNull_ThrowsException()
        {
            Func<Task> action = (async () =>
            {
                await _leaderboardService.GetStandingsForEachTeamAsync();
            });


            await action.Should().ThrowAsync<SeasonNotFoundException>();
        }

        [Fact]
        public async Task GetStandingsForEachTeamAsync_NoStandings_StandingsAreEmpty()
        {
            var season = new Season
            {
                Name = "2024/2025",
                StartAt = new DateTime(2024, 1, 1),
                EndAt = new DateTime(2025, 1, 1)
            };

            _context.Seasons.Add(season);
            await _context.SaveChangesAsync();


            var standings = await _leaderboardService.GetStandingsForEachTeamAsync();


            standings.Should().BeEmpty();
        }

        [Fact]
        public async Task GetStandingsForEachTeamAsync_ValidData_ReturnsStandings()
        {
            var season = new Season
            {
                Name = "2024/2025",
                StartAt = new DateTime(2024, 1, 1),
                EndAt = new DateTime(2025, 1, 1)
            };

            var team = new Team
            {
                Name = "Team1"
            };

            var stage = new Stage
            {
                Name = "Stage1",
                Season = season
            };

            var standing = new Standing
            {
                Place = 1,
                Team = team,
                Stage = stage
            };

            _context.Seasons.Add(season);
            _context.Teams.Add(team);
            _context.Stages.Add(stage);
            _context.Standings.Add(standing);
            await _context.SaveChangesAsync();


            var standings = await _leaderboardService.GetStandingsForEachTeamAsync();

            var actualTeam = standing.Team;
            var expectedTeam = standings[0].Team;

            actualTeam.Name.Should().Be(expectedTeam.Name);
        }
        #endregion
    }
}
