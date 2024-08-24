using LeagueLeaders.Application;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<IActionResult> GetClosestMatchesAsync()
        {
            var matches = await _scheduleSerivce.GetClosestMatchesAsync();

            return Ok(matches);
        }
    }
}
