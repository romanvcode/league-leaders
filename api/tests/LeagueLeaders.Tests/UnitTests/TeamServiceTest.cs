using FluentAssertions;
using FluentAssertions.Execution;
using LeagueLeaders.Application;
using LeagueLeaders.Application.Teams;
using LeagueLeaders.Domain;
using LeagueLeaders.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace LeagueLeaders.Tests.UnitTests;

public class TeamServiceTest : IDisposable
{
    private readonly LeagueLeadersDbContext _context;

    public TeamServiceTest()
    {
        var options = new DbContextOptionsBuilder<LeagueLeadersDbContext>()
            .UseInMemoryDatabase(databaseName: "LeagueLeadersTeamDB")
            .Options;

        _context = new LeagueLeadersDbContext(options);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
    }

    #region GetTeamAsync
    [Fact]
    public async void GetTeamAsync_InvalidTeamId_ThrowsException()
    {
        var _teamService = new TeamService(_context);

        int teamId = -1;


        var getTeam = () => _teamService.GetTeamAsync(teamId);


        await getTeam.Should().ThrowAsync<TeamNotFoundException>();
    }

    [Fact]
    public async void GetTeamAsync_ValidTeamId_ReturnsTeam()
    {
        var _teamService = new TeamService(_context);

        var expectedTeam = new Team
        {
            Name = "Team 1"
        };

        _context.Teams.Add(expectedTeam);
        await _context.SaveChangesAsync();


        var actualTeam = await _teamService.GetTeamAsync(expectedTeam.Id);


        using (new AssertionScope())
        {
            actualTeam.Should().NotBeNull();
            actualTeam?.Id.Should().Be(expectedTeam.Id);
            actualTeam?.Name.Should().Be(expectedTeam.Name);
        }
    }
    #endregion

    #region GetTeamPlayersAsync
    [Fact]
    public async void GetTeamPlayersAsync_InvalidTeamId_ThrowsException()
    {
        var _teamService = new TeamService(_context);

        int teamId = -1;


        var getTeam = () => _teamService.GetTeamPlayersAsync(teamId);


        await getTeam.Should().ThrowAsync<TeamNotFoundException>();
    }

    [Fact]
    public async void GetTeamPlayersAsync_NoPlayers_ReturnsEmptyList()
    {
        var _teamService = new TeamService(_context);

        var team = new Team
        {
            Name = "Team 1"
        };

        _context.Teams.Add(team);
        await _context.SaveChangesAsync();


        var players = await _teamService.GetTeamPlayersAsync(team.Id);


        players.Should().BeEmpty();
    }

    [Fact]
    public async void GetTeamPlayersAsync_ValidTeamWithPlayers_ReturnsPlayers()
    {
        var _teamService = new TeamService(_context);

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


        var actualPlayers = await _teamService.GetTeamPlayersAsync(team.Id);


        actualPlayers.Should().BeEquivalentTo(expectedPlayers, options => options.Including(p => p.Id).Including(p => p.Name));
    }
    #endregion

    #region GetMatchHistoryForTeamAsync
    [Fact]
    public async void GetMatchHistoryForTeamAsync_InvalidTeam_ThrowsException()
    {
        var _teamService = new TeamService(_context);

        var season = new Season
        {
            Name = "2024/2025",
            StartAt = new DateTime(2024, 1, 1),
            EndAt = new DateTime(2025, 1, 1)
        };

        _context.Seasons.Add(season);
        await _context.SaveChangesAsync();

        int teamId = -1;


        var getMatchHistory = () => _teamService.GetMatchHistoryForTeamAsync(teamId);


        await getMatchHistory.Should().ThrowAsync<TeamNotFoundException>();
    }

    [Fact]
    public async void GetMatchHistoryForTeamAsync_CurrentSeasonIsNull_ThrowsException()
    {
        var _teamService = new TeamService(_context);

        var team = new Team
        {
            Name = "Team 1"
        };

        _context.Teams.Add(team);
        await _context.SaveChangesAsync();


        var getMatchHistory = () => _teamService.GetMatchHistoryForTeamAsync(team.Id);


        await getMatchHistory.Should().ThrowAsync<SeasonNotFoundException>();
    }

    [Fact]
    public async void GetMatchHistoryForTeamAsync_NoMatches_ReturnsEmptyList()
    {
        var _teamService = new TeamService(_context);

        var season = new Season
        {
            Name = "2024/2025",
            StartAt = new DateTime(2024, 1, 1),
            EndAt = new DateTime(2025, 1, 1)
        };

        var team = new Team
        {
            Name = "Team 1"
        };

        _context.Seasons.Add(season);
        _context.Teams.Add(team);
        await _context.SaveChangesAsync();


        var actualMatches = await _teamService.GetMatchHistoryForTeamAsync(team.Id);


        actualMatches.Should().BeEmpty();
    }

    [Fact]
    public async void GetMatchHistoryForTeamAsync_ValidData_ReturnsMatches()
    {
        var _teamService = new TeamService(_context);

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
            HomeTeam = team1,
            AwayTeam = team2,
            Date = new DateTime(2024, 1, 1),
            Stage = stage
        };
        var match2 = new Match
        {
            HomeTeam = team2,
            AwayTeam = team1,
            Date = new DateTime(2024, 1, 2),
            Stage = stage
        };

        _context.Seasons.Add(season);
        _context.Teams.AddRange(team1, team2);
        _context.Matches.AddRange(match1, match2);
        await _context.SaveChangesAsync();

        var expectedMatches = new List<Match>() { match2, match1 };


        var actualMatches = await _teamService.GetMatchHistoryForTeamAsync(team1.Id);


        actualMatches.Should().BeEquivalentTo(expectedMatches, options => options.Including(m => m.Id).Including(m => m.HomeTeam.Id).Including(m => m.AwayTeam.Id).Including(m => m.Date));
    }
    #endregion

    #region GetTeamsBySearchTermAsync
    [Fact]
    public async void GetTeamsBySearchTermAsync_EmptySearchTerm_ThrowsException()
    {
        var _teamService = new TeamService(_context);

        string searchTerm = "";


        var getTeams = () => _teamService.GetTeamsBySearchTermAsync(searchTerm);


        await getTeams.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void GetTeamsBySearchTermAsync_NoTeams_ReturnsEmptyList()
    {
        var _teamService = new TeamService(_context);

        string searchTerm = "Team";


        var actualTeams = await _teamService.GetTeamsBySearchTermAsync(searchTerm);


        actualTeams.Should().BeEmpty();
    }

    [Fact]
    public async void GetTeamsBySearchTermAsync_ValidData_ReturnsTeams()
    {
        var _teamService = new TeamService(_context);

        var team1 = new Team
        {
            Name = "Team 1"
        };
        var team2 = new Team
        {
            Name = "Team 2"
        };

        _context.Teams.AddRange(team1, team2);
        await _context.SaveChangesAsync();

        var expectedTeams = new List<Team>() { team1, team2 };


        var actualTeams = await _teamService.GetTeamsBySearchTermAsync("Team");


        actualTeams.Should().BeEquivalentTo(expectedTeams, options => options.Including(t => t.Id).Including(t => t.Name));
    }
    #endregion
}