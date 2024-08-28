using LeagueLeaders.Application;
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

        public async void Dispose()
        {
            await _context.Database.EnsureDeletedAsync();
        }

        #region GetStandingsForEachTeamAsync
        [Fact]
        public async Task GetStandingsForEachTeamAsync_CurrentSeasonIsNull_ThrowsException()
        {
            // Arrange

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _leaderboardService.GetStandingsForEachTeamAsync());

            // Assert
            Assert.Equal($"There is no season which will run during {DateTime.UtcNow}", exception.Message);
        }

        [Fact]
        public async Task GetStandingsForEachTeamAsync_NoStandings_StandingsAreEmpty()
        {
            // Arrange
            var season = new Season
            {
                Name = "2024/2025",
                StartAt = DateTime.Parse("2024-01-01"),
                EndAt = DateTime.Parse("2025-01-01")
            };

            _context.Seasons.Add(season);
            await _context.SaveChangesAsync();

            // Act
            var standings = await _leaderboardService.GetStandingsForEachTeamAsync();

            // Assert
            Assert.Empty(standings);
        }

        [Fact]
        public async Task GetStandingsForEachTeamAsync_ValidData_ReturnsStandings()
        {
            // Arrange
            var season = new Season
            {
                Name = "2024/2025",
                StartAt = DateTime.Parse("2024-01-01"),
                EndAt = DateTime.Parse("2025-01-01")
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

            // Act
            var standings = await _leaderboardService.GetStandingsForEachTeamAsync();

            // Assert
            Assert.Equal(standing.Place, standings[0].Place);
            Assert.Equal(standing.Team.Name, standings[0].Team.Name);
        }
        #endregion
    }
}
