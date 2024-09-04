using LeagueLeaders.Domain;

namespace LeagueLeaders.Application.Schedule;

public interface IScheduleService
{
    Task<List<Match>> GetClosestMatchesAsync();
}
