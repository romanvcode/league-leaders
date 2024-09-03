using LeagueLeaders.Domain;

namespace LeagueLeaders.Application.Leaderboard;

public interface ILeaderboardService
{
    Task<List<Standing>> GetStandingsForEachTeamAsync();
}
