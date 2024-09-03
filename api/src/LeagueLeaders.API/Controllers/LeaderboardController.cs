using LeagueLeaders.Application.Leaderboard;
using LeagueLeaders.Domain;
using Microsoft.AspNetCore.Mvc;

namespace LeagueLeaders.API.Controllers;

[Route("api/leaderboard")]
[ApiController]
public class LeaderboardController : ControllerBase
{
    private readonly ILeaderboardService _leaderboardService;

    public LeaderboardController(ILeaderboardService leaderboardService)
    {
        _leaderboardService = leaderboardService;
    }

    /// <summary>
    /// Get Standings for each Team.
    /// </summary>
    /// <returns>List of <see cref="Standing"/>s.</returns>
    [HttpGet("standings")]
    [ProducesResponseType(typeof(List<Standing>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<List<Standing>> GetStandingsAsync()
    {
        var standings = await _leaderboardService.GetStandingsForEachTeamAsync();

        return standings;
    }
}
