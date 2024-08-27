using LeagueLeaders.Domain;

namespace LeagueLeaders.Application
{
    public interface ITeamService
    {
        Task<Team?> GetTeamAsync(int teamId);

        Task<List<Player>> GetTeamPlayersAsync(int teamId);

        Task<List<Match>> GetLastFiveTeamMatches(int teamId);

        Task<List<Team>> GetTeamsBySearchTerm(string searchTerm);
    }
}
