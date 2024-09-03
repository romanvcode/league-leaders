using LeagueLeaders.Domain;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using LeagueLeaders.API.Validators;
using LeagueLeaders.Application.Teams;

namespace LeagueLeaders.API.Controllers;

[Route("api/teams")]
[ApiController]
public class TeamController : ControllerBase
{
    private readonly ITeamService _teamService;

    public TeamController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    /// <summary>
    /// Get single Team by Team ID.
    /// </summary>
    /// <param name="teamId">The ID of the Team.</param>
    /// <returns>The found <see cref="Team"/>.</returns>
    [HttpGet("{teamId}")]
    [ProducesResponseType(typeof(Team), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<Team> GetTeamAsync(int teamId)
    {
        var validator = new TeamIdValidator();
        var validationResult = validator.Validate(teamId);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var team = await _teamService.GetTeamAsync(teamId);

        return team;
    }

    /// <summary>
    /// Get List of Players of the Team by Team ID.
    /// </summary>
    /// <param name="teamId">The ID of the Team.</param>
    /// <returns>The list of found <see cref="Player"/>s.</returns>
    [HttpGet("{teamId}/players")]
    [ProducesResponseType(typeof(List<Player>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<List<Player>> GetTeamPlayersAsync(int teamId)
    {
        var validator = new TeamIdValidator();
        var validationResult = validator.Validate(teamId);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var players = await _teamService.GetTeamPlayersAsync(teamId);

        return players;
    }

    /// <summary>
    /// Get the last 5 Matches of the Team by Team ID.
    /// </summary>
    /// <param name="teamId">The ID of the Team.</param>
    /// <returns>The list of found <see cref="Match"/>es.</returns>
    [HttpGet("{teamId}/matches")]
    [ProducesResponseType(typeof(List<Match>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<List<Match>> GetLastFiveTeamMatches(int teamId)
    {
        var validator = new TeamIdValidator();
        var validationResult = validator.Validate(teamId);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var matches = await _teamService.GetMatchHistoryForTeamAsync(teamId);

        return matches;
    }

    /// <summary>
    /// Get List of Teams by Search Term.
    /// </summary>
    /// <param name="searchTerm">The search term to search for.</param>
    /// <returns>The list of found <see cref="Team"/>s.</returns>
    [HttpGet("search/{searchTerm}")]
    [ProducesResponseType(typeof(List<Team>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<List<Team>> GetTeamsBySerchTerm(string searchTerm)
    {
        var teams = await _teamService.GetTeamsBySearchTermAsync(searchTerm);

        return teams;
    }
}
