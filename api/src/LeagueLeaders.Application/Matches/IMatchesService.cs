using LeagueLeaders.Domain;

namespace LeagueLeaders.Application.Matches;
public interface IMatchesService
{
    Task<List<TeamStat>> GetTeamStatsForMatchAsync(int matchId);
}
