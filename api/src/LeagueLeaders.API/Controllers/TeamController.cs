using LeagueLeaders.Application;
using LeagueLeaders.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LeagueLeaders.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpGet("{teamId}")]
        public async Task<ActionResult<Team>> GetTeamAsync(int teamId)
        {
            try
            {
                var team = await _teamService.GetTeamAsync(teamId);

                if (team == null)
                {
                    return NotFound("Team not found.");
                }

                return Ok(team);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{teamId}/players")]
        public async Task<ActionResult<List<Player>>> GetTeamPlayersAsync(int teamId)
        {
            try
            {
                var players = await _teamService.GetTeamPlayersAsync(teamId);

                if (players.IsNullOrEmpty())
                {
                    return NotFound("Invalid team ID or no players found for the team.");
                }

                return Ok(players);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{teamId}/matches")]
        public async Task<ActionResult<List<Match>>> GetLastFiveTeamMatches(int teamId)
        {
            try
            {
                var matches = await _teamService.GetLastFiveTeamMatches(teamId);

                if (matches.IsNullOrEmpty())
                {
                    return NotFound("Invalid team ID or no matches found for the team.");
                }

                return Ok(matches);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<Team>>> GetTeamsBySerchTerm(string searchTerm)
        {
            try
            {
                var teams = await _teamService.GetTeamsBySearchTerm(searchTerm);

                if (teams.IsNullOrEmpty())
                {
                    return NotFound("No teams found.");
                }

                return Ok(teams);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
