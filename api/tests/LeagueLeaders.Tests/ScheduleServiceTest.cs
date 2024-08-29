using LeagueLeaders.Application;
using LeagueLeaders.Domain;
using LeagueLeaders.Infrastructure;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using LeagueLeaders.Application.Exceptions;

namespace LeagueLeaders.Tests
{
    public class ScheduleServiceTests : IDisposable
    {
        private readonly LeagueLeadersDbContext _context;
        private readonly ScheduleSerivce _scheduleService;

        public ScheduleServiceTests()
        {
            var options = new DbContextOptionsBuilder<LeagueLeadersDbContext>()
                .UseInMemoryDatabase(databaseName: "LeagueLeadersScheduleDB")
                .Options;

            _context = new LeagueLeadersDbContext(options);
            _scheduleService = new ScheduleSerivce(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }

        [Fact]
        public async Task GetClosestMatches_CurrentSeasonIsNull_ThrowsException()
        {
            Func<Task> action = (async () =>
            {
                await _scheduleService.GetClosestMatchesAsync();
            });


            await action.Should().ThrowAsync<SeasonNotFoundException>();
        }

        [Fact]
        public async Task GetClosestMatches_CurrentStageIsNull_ThrowsException()
        {
            var season = new Season
            {
                Name = "2024/2025",
                StartAt = new DateTime(2024, 1, 1),
                EndAt = new DateTime(2025, 1, 1)
            };

            _context.Seasons.Add(season);
            await _context.SaveChangesAsync();


            Func<Task> action = (async () =>
            {
                await _scheduleService.GetClosestMatchesAsync();
            });


            await action.Should().ThrowAsync<StageNotFoundException>();
        }

        [Fact]
        public async Task GetClosestMatches_ValidData_ReturnsMatches()
        {
            var season = new Season
            {
                Name = "2024/2025",
                StartAt = new DateTime(2024, 1, 1),
                EndAt = new DateTime(2025, 1, 1)
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

            var expectedMatches = new List<Match> { match1, match2 };

            _context.Seasons.Add(season);
            _context.Stages.Add(stage);
            _context.Teams.AddRange(team1, team2);
            _context.Matches.AddRange(match1, match2);
            await _context.SaveChangesAsync();


            var actualMatches = await _scheduleService.GetClosestMatchesAsync();


            actualMatches.Should()
                .BeEquivalentTo(expectedMatches, options => options.Including(m => m.Id));
        }
    }
}