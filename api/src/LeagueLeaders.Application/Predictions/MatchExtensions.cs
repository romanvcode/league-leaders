using LeagueLeaders.Domain;

namespace LeagueLeaders.Application.Predictions;

public static class MatchExtensions
{
    public static int TotalGoalsScoredBy(this IEnumerable<Match> matches, int teamId)
    {
        return matches.Sum(m =>
            m.HomeTeamId == teamId ? m.HomeTeamScore :
            m.AwayTeamId == teamId ? m.AwayTeamScore : 0
        );
    }

    public static int TotalGoalsConcededBy(this IEnumerable<Match> matches, int teamId)
    {
        return matches.Sum(m =>
            m.HomeTeamId == teamId ? m.AwayTeamScore :
            m.AwayTeamId == teamId ? m.HomeTeamScore : 0
        );
    }
}
