using FluentValidation;
using LeagueLeaders.API.Validators;
using LeagueLeaders.Application.Matches;
using LeagueLeaders.Domain;
using Microsoft.AspNetCore.Mvc;

namespace LeagueLeaders.API.Controllers;

[Route("api/matches")]
[ApiController]
public class MatchesController : ControllerBase
{
    private readonly IMatchesService _matchesService;

    public MatchesController(IMatchesService matchesService)
    {
        _matchesService = matchesService;
    }

    /// <summary>
    /// Get the Teams Stats by Match ID.
    /// </summary>
    /// <param name="matchId">The ID of the Match.</param>
    /// <returns>The list of found <see cref="TeamStat"/>s.</returns>
    [HttpGet("{matchId}/teams-stats")]
    [ProducesResponseType(typeof(List<Match>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<List<TeamStat>> GetMatchStatsAsync(int matchId)
    {
        var validator = new MatchIdValidator();
        var validationResult = validator.Validate(matchId);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var teamsStats = await _matchesService.GetTeamStatsForMatchAsync(matchId);

        return teamsStats;
    }
}
