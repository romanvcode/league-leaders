using LeagueLeaders.Application;
using LeagueLeaders.Application.Exceptions;
using LeagueLeaders.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LeagueLeaders.API.Controllers
{
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
        public async Task<ActionResult<Team>> GetTeamAsync(int teamId)
        {
            if (teamId <= 0)
            {
                return BadRequest("Invalid team ID.");
            }

            try
            {
                var team = await _teamService.GetTeamAsync(teamId);

                if (team == null)
                {
                    return NotFound("Team not found.");
                }

                return Ok(team);
            }
            catch (TeamNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, 
                    statusCode: StatusCodes.Status500InternalServerError);
            }
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
        public async Task<ActionResult<List<Player>>> GetTeamPlayersAsync(int teamId)
        {
            if (teamId <= 0) 
            {
                return BadRequest("Invalid team ID.");
            }

            try
            {
                var players = await _teamService.GetTeamPlayersAsync(teamId);

                if (players.IsNullOrEmpty())
                {
                    return NotFound("Invalid team ID or no players found for the team.");
                }

                return Ok(players);
            }
            catch (TeamNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
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
        public async Task<ActionResult<List<Match>>> GetLastFiveTeamMatches(int teamId)
        {
            if (teamId <= 0)
            {
                return BadRequest("Invalid team ID.");
            }

            try
            {
                var matches = await _teamService.GetMatchHistoryForTeamAsync(teamId);

                if (matches.IsNullOrEmpty())
                {
                    return NotFound("No matches found for the team.");
                }

                return Ok(matches);
            }
            catch (TeamNotFoundException ex)
            {
                return NotFound(ex.Message);
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
        public async Task<ActionResult<List<Team>>> GetTeamsBySerchTerm(string searchTerm)
        {
            try
            {
                var teams = await _teamService.GetTeamsBySearchTermAsync(searchTerm);

                if (teams.IsNullOrEmpty())
                {
                    return NotFound("No teams found.");
                }

                return Ok(teams);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
