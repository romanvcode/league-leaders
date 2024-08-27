using LeagueLeaders.Domain;

namespace LeagueLeaders.Application
{
    public interface ILeaderboardService
    {
        Task<List<Standing>> GetStandingsForEachTeamAsync();
    }
}
