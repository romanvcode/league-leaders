using LeagueLeaders.Application;
using LeagueLeaders.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LeagueLeaders.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleSerivce _scheduleSerivce;

        public ScheduleController(IScheduleSerivce scheduleSerivce)
        {
            _scheduleSerivce = scheduleSerivce;
        }

        [HttpGet("Matches")]
        public async Task<ActionResult<List<Match>>> GetClosestMatchesAsync()
        {
            try
            {
                var matches = await _scheduleSerivce.GetClosestMatchesAsync();

                if (matches.IsNullOrEmpty())
                {
                    return NotFound("No matches found.");
                }

                return Ok(matches);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
