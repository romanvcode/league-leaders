using LeagueLeaders.Application;
using LeagueLeaders.Domain;
using LeagueLeaders.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;

namespace LeagueLeaders.Tests
{
    public class ScheduleServiceTests : IDisposable
    {
        private readonly LeagueLeadersDbContext _context;
        private readonly ScheduleSerivce _scheduleService;

        public ScheduleServiceTests()
        {
            var options = new DbContextOptionsBuilder<LeagueLeadersDbContext>()
                .UseInMemoryDatabase(databaseName: "LeagueLeadersDatabase")
                .Options;

            _context = new LeagueLeadersDbContext(options);
            _scheduleService = new ScheduleSerivce(_context);
        }

        public async void Dispose()
        {
            await _context.Database.EnsureDeletedAsync();
        }

        [Fact]
        public async Task GetClosestMatches_CurrentSeasonIsNull_ThrowsException()
        {
            // Arrange

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _scheduleService.GetClosestMatchesAsync());

            // Assert
            Assert.Equal($"There is no season which will run during {DateTime.UtcNow}", exception.Message);
        }

        [Fact]
        public async Task GetClosestMatches_CurrentStageIsNull_ThrowsException()
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

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _scheduleService.GetClosestMatchesAsync());

            // Assert
            Assert.Equal($"There is no stage which will run during current season: {season.Name}", exception.Message);
        }

        [Fact]
        public async Task GetClosestMatches_ValidData_ReturnsMatches()
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
                Date = DateTime.UtcNow.AddDays(1),
                HomeTeam = team1,
                AwayTeam = team2,
                Stage = stage
            };

            var match2 = new Match
            {
                Date = DateTime.UtcNow.AddDays(2),
                HomeTeam = team2,
                AwayTeam = team1,
                Stage = stage
            };

            _context.Seasons.Add(season);
            _context.Stages.Add(stage);
            _context.Teams.AddRange(team1, team2);
            _context.Matches.AddRange(match1, match2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _scheduleService.GetClosestMatchesAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(match1.Id, result[0].Id);
            Assert.Equal(match2.Id, result[1].Id);
        }
    }
}
