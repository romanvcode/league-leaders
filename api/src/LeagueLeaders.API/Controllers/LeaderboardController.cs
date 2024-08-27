using LeagueLeaders.Application;
using LeagueLeaders.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LeagueLeaders.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        private readonly ILeaderboardService _leaderboardService;

        public LeaderboardController(ILeaderboardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
        }

        [HttpGet]
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
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
