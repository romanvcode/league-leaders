using LeagueLeaders.Domain;

namespace LeagueLeaders.Application
{
    public interface IScheduleSerivce
    {
        Task<List<Match>> GetClosestMatchesAsync();
    }
}
