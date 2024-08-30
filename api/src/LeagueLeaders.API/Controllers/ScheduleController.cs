using LeagueLeaders.Application;
using LeagueLeaders.Application.Exceptions;
using LeagueLeaders.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LeagueLeaders.API.Controllers
{
    [Route("api/schedule")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleSerivce _scheduleSerivce;

        public ScheduleController(IScheduleSerivce scheduleSerivce)
        {
            _scheduleSerivce = scheduleSerivce;
        }

        /// <summary>
        /// Get five closest Matches of the current Stage.
        /// </summary>
        /// <returns>List of <see cref="Match"/>es.</returns>
        [HttpGet("matches")]
        [ProducesResponseType(typeof(List<Match>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
            catch (SeasonNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (StageNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
