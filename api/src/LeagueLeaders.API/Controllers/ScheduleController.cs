using LeagueLeaders.Application;
using LeagueLeaders.Domain;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<List<Match>> GetClosestMatchesAsync()
        {
            var matches = await _scheduleSerivce.GetClosestMatchesAsync();

            return matches;
        }
    }
}
