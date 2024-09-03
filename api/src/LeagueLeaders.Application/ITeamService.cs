using LeagueLeaders.Domain;

namespace LeagueLeaders.Application
{
    public interface ITeamService
    {
        Task<Team> GetTeamAsync(int teamId);

        Task<List<Player>> GetTeamPlayersAsync(int teamId);

        Task<List<Match>> GetMatchHistoryForTeamAsync(int teamId, int lastMatches = 5);

        Task<List<Team>> GetTeamsBySearchTermAsync(string searchTerm);
    }
}
