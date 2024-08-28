using LeagueLeaders.Application;
using LeagueLeaders.Domain;
using LeagueLeaders.Infrastructure;
using Microsoft.EntityFrameworkCore;
namespace LeagueLeaders.Tests
{
    public class TeamServiceTest
    {
        private readonly LeagueLeadersDbContext _context;
        private readonly TeamService _teamService;
        public TeamServiceTest()
        {
            var options = new DbContextOptionsBuilder<LeagueLeadersDbContext>()
                .UseInMemoryDatabase(databaseName: "LeagueLeadersTeamDB")
                .Options;
            _context = new LeagueLeadersDbContext(options);
            _teamService = new TeamService(_context);
        }
        #region GetTeamAsync
        [Fact]
        public async void GetTeamAsync_InvalidTeamId_ThrowsException()
        {
            // Arrange
            int teamId = -1;
            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => _teamService.GetTeamAsync(teamId));
            // Assert
            Assert.Equal($"Team with Id {teamId} not found.", exception.Message);
            await _context.Database.EnsureDeletedAsync();
        }
        [Fact]
        public async void GetTeamAsync_ValidTeamId_ReturnsTeam()
        {
            // Arrange
            var expectedTeam = new Team
            {
                Name = "Team 1"
            };
            _context.Teams.Add(expectedTeam);
            await _context.SaveChangesAsync();
            // Act
            var actualTeam = await _teamService.GetTeamAsync(expectedTeam.Id);
            // Assert
            Assert.Equal(expectedTeam.Id, actualTeam.Id);
            Assert.Equal(expectedTeam.Name, actualTeam.Name);
            await _context.Database.EnsureDeletedAsync();
        }
        #endregion
        #region GetTeamPlayersAsync
        [Fact]
        public async void GetTeamPlayersAsync_InvalidTeamId_ThrowsException()
        {
            // Arrange
            int teamId = -1;
            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => _teamService.GetTeamPlayersAsync(teamId));
            // Assert
            Assert.Equal($"Team with Id {teamId} not found.", exception.Message);
            await _context.Database.EnsureDeletedAsync();
        }
        [Fact]
        public async void GetTeamPlayersAsync_NoPlayers_ReturnsEmptyList()
        {
            // Arrange
            var expectedTeam = new Team
            {
                Name = "Team 1"
            };
            _context.Teams.Add(expectedTeam);
            await _context.SaveChangesAsync();
            // Act
            var actualTeam = await _teamService.GetTeamPlayersAsync(expectedTeam.Id);
            // Assert
            Assert.Empty(actualTeam);
            await _context.Database.EnsureDeletedAsync();
        }
        [Fact]
        public async void GetTeamPlayersAsync_ValidTeamWithPlayers_ReturnsPlayers()
        {
            // Arrange
            var team = new Team
            {
                Name = "Team 1"
            };
            var player1 = new Player
            {
                Name = "Player 1",
                Team = team
            };
            var player2 = new Player
            {
                Name = "Player 2",
                Team = team
            };
            _context.Teams.Add(team);
            _context.Players.AddRange(player1, player2);
            await _context.SaveChangesAsync();
            var expectedPlayers = team.Players;
            // Act
            var actualPlayers = await _teamService.GetTeamPlayersAsync(team.Id);
            // Assert
            Assert.Equal(expectedPlayers.Count, actualPlayers.Count);
            Assert.Equal(expectedPlayers[0].Id, actualPlayers[0].Id);
            Assert.Equal(expectedPlayers[0].Name, actualPlayers[0].Name);
            Assert.Equal(expectedPlayers[1].Id, actualPlayers[1].Id);
            Assert.Equal(expectedPlayers[1].Name, actualPlayers[1].Name);
            await _context.Database.EnsureDeletedAsync();
        }
        #endregion
        #region GetMatchHistoryForTeamAsync
        [Fact]
        public async void GetMatchHistoryForTeamAsync_InvalidTeam_ThrowsException()
        {
            // Arrange
            int teamId = -1;
            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => _teamService.GetMatchHistoryForTeamAsync(teamId));
            // Assert
            Assert.Equal($"Team with Id {teamId} not found.", exception.Message);
            await _context.Database.EnsureDeletedAsync();
        }
        [Fact]
        public async void GetMatchHistoryForTeamAsync_CurrentSeasonIsNull_ThrowsException()
        {
            // Arrange
            var team = new Team
            {
                Name = "Team 1"
            };
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => _teamService.GetMatchHistoryForTeamAsync(team.Id));
            // Assert
            Assert.Equal($"There is no season which will run during {DateTime.UtcNow}", exception.Message);
            await _context.Database.EnsureDeletedAsync();
        }
        [Fact]
        public async void GetMatchHistoryForTeamAsync_NoMatches_ReturnsEmptyList()
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
                Name = "Team 1"
            };
            _context.Seasons.Add(season);
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            // Act
            var actualMatches = await _teamService.GetMatchHistoryForTeamAsync(team.Id);
            // Assert
            Assert.Empty(actualMatches);
            await _context.Database.EnsureDeletedAsync();
        }
        [Fact]
        public async void GetMatchHistoryForTeamAsync_ValidData_ReturnsMatches()
        {
            // Arrange
            var season = new Season
            {
                Name = "2024/2025",
                StartAt = DateTime.Parse("2024-01-01"),
                EndAt = DateTime.Parse("2025-01-01")
            };
            var stage = new Stage
            {
                Name = "Group Stage",
                Season = season
            };
            var team1 = new Team
            {
                Name = "Team 1"
            };
            var team2 = new Team
            {
                Name = "Team 2"
            };
            var match1 = new Match
            {
                HomeTeam = team1,
                AwayTeam = team2,
                Date = DateTime.Parse("2024-01-01"),
                Stage = stage
            };
            var match2 = new Match
            {
                HomeTeam = team2,
                AwayTeam = team1,
                Date = DateTime.Parse("2024-01-02"),
                Stage = stage
            };
            _context.Seasons.Add(season);
            _context.Teams.AddRange(team1, team2);
            _context.Matches.AddRange(match1, match2);
            await _context.SaveChangesAsync();
            var expectedMatches = new List<Match>() { match2, match1 };
            // Act
            var actualMatches = await _teamService.GetMatchHistoryForTeamAsync(team1.Id);
            // Assert
            Assert.Equal(expectedMatches.Count, actualMatches.Count);
            Assert.Equal(expectedMatches[0].Id, actualMatches[0].Id);
            Assert.Equal(expectedMatches[0].HomeTeam.Id, actualMatches[0].HomeTeam.Id);
            Assert.Equal(expectedMatches[0].AwayTeam.Id, actualMatches[0].AwayTeam.Id);
            Assert.Equal(expectedMatches[0].Date, actualMatches[0].Date);
            Assert.Equal(expectedMatches[1].Id, actualMatches[1].Id);
            Assert.Equal(expectedMatches[1].HomeTeam.Id, actualMatches[1].HomeTeam.Id);
            Assert.Equal(expectedMatches[1].AwayTeam.Id, actualMatches[1].AwayTeam.Id);
            Assert.Equal(expectedMatches[1].Date, actualMatches[1].Date);
            await _context.Database.EnsureDeletedAsync();
        }
        #endregion
    }
}