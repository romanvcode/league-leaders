using LeagueLeaders.Domain;

namespace LeagueLeaders.Application.Matches;
public static class TeamStatExtensions
{
    public static List<TeamStat> AsList(this List<TeamStat> stats)
    {
        var random = new Random();

        if (stats.Count != 2)
            return stats;

        int team1Possession = random.Next(30, 71);
        int team2Possession = 100 - team1Possession;

        stats[0].Possession = team1Possession;
        stats[1].Possession = team2Possession;

        foreach (var stat in stats)
        {
            stat.Corners = random.Next(0, 5);
            stat.Offsides = random.Next(0, 7);
            stat.Fouls = random.Next(2, 12);
            stat.Shots = random.Next(4, 16);
            stat.ShotsOnTarget = random.Next(1, 8);
        }

        return stats;
    }
}
