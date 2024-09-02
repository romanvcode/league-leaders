using LeagueLeaders.Domain;

namespace LeagueLeaders.Application
{
    public interface IScheduleService
    {
        Task<List<Match>> GetClosestMatchesAsync();
    }
}
