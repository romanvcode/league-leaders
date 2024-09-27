using LeagueLeaders.Domain;
using LeagueLeaders.Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LeagueLeaders.Tests.IntegrationTests;

public class LeagueLeadersWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            var descripter = services
            .SingleOrDefault(s => 
            s.ServiceType == typeof(DbContextOptions<LeagueLeadersDbContext>));

            if (descripter != null)
            {
                services.Remove(descripter);
            }

            services.AddDbContext<LeagueLeadersDbContext>(options =>
            {
                options.UseInMemoryDatabase("DatabaseForTesting");
            });

            var serviceProvider = services.BuildServiceProvider();
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<LeagueLeadersDbContext>();
                SeedTestData(dbContext);
            }
        });
    }

    private void SeedTestData(LeagueLeadersDbContext context)
    {
        context.Database.EnsureCreated();

        var season = new Season
        {
            Id = 1,
            Name = "2023/2024",
            StartAt = DateTime.UtcNow.AddMonths(-2),
            EndAt = DateTime.UtcNow.AddMonths(10)
        };

        var stage = new Stage
        {
            Id = 1,
            Name = "Group Stage",
            SeasonId = season.Id
        };

        var team1 = new Team { Id = 1, Name = "Team A", Abbreviation = "TA", Country = "Country A" };
        var team2 = new Team { Id = 2, Name = "Team B", Abbreviation = "TB", Country = "Country B" };

        var player1 = new Player { Id = 1, Name = "Player A", TeamId = team1.Id };
        var player2 = new Player { Id = 2, Name = "Player B", TeamId = team2.Id };

        var standing1 = new Standing { Id = 1, Place = 1, TeamId = team1.Id, StageId = stage.Id };
        var standing2 = new Standing { Id = 2, Place = 2, TeamId = team2.Id, StageId = stage.Id };

        var match1 = new Match
        {
            Id = 1,
            HomeTeamId = team1.Id,
            AwayTeamId = team2.Id,
            StageId = stage.Id,
            Date = DateTime.UtcNow.AddDays(1),
            HomeTeamScore = 2,
            AwayTeamScore = 1
        };

        var match2 = new Match
        {
            Id = 2,
            HomeTeamId = team2.Id,
            AwayTeamId = team1.Id,
            StageId = stage.Id,
            Date = DateTime.UtcNow.AddDays(-1),
            HomeTeamScore = 1,
            AwayTeamScore = 2
        };

        context.Seasons.Add(season);
        context.Stages.Add(stage);
        context.Teams.AddRange(team1, team2);
        context.Players.AddRange(player1, player2);
        context.Standings.AddRange(standing1, standing2);
        context.Matches.AddRange(match1, match2);

        context.SaveChanges();
    }
}
