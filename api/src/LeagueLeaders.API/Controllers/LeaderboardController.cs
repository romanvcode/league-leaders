<<<<<<< Updated upstream
﻿using LeagueLeaders.Application;
using LeagueLeaders.Application.Exceptions;
=======
﻿using LeagueLeaders.Application.Leaderboard;
>>>>>>> Stashed changes
using LeagueLeaders.Domain;
using Microsoft.AspNetCore.Mvc;

namespace LeagueLeaders.API.Controllers
{
    [Route("api/leaderboard")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        private readonly ILeaderboardService _leaderboardService;

<<<<<<< Updated upstream
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
        public async Task<ActionResult<List<Standing>>> GetStandingsAsync()
        {
            try
            {
                var standings = await _leaderboardService.GetStandingsForEachTeamAsync();

                if (standings.IsNullOrEmpty())
                {
                    return NotFound("No standings found.");
                }

                return Ok(standings);
            }
            catch (SeasonNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
=======
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
>>>>>>> Stashed changes
    }
}
