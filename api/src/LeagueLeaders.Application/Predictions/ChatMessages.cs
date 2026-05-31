namespace LeagueLeaders.Application.Predictions;
public static class ChatMessages
{
    public const string HomeTeam = "Home Team";
    public const string AwayTeam = "Away Team";

    public static string GetPredictionRequest(
        double homeTeamGoalsScoredAvg,
        double homeTeamGoalsConcededAvg,
        double awayTeamGoalsScoredAvg,
        double awayTeamGoalsConcededAvg,
        List<((string, int HomeTeamScore), (string, int AwayteamScore))> headToHeadResults)
    {
        return $$"""
                 You are an expert football analyst and sports bettor using Poisson distribution and tactical analysis.

                 # Task
                 Analyze the upcoming match statistics to predict the EXACT score. 

                 # Statistics
                 - Home Team (Avg Goals Scored): {{homeTeamGoalsScoredAvg:F2}}
                 - Home Team (Avg Goals Conceded): {{homeTeamGoalsConcededAvg:F2}}
                 - Away Team (Avg Goals Scored): {{awayTeamGoalsScoredAvg:F2}}
                 - Away Team (Avg Goals Conceded): {{awayTeamGoalsConcededAvg:F2}}
                 - Recent Head-to-Head: {{headToHeadResults}}

                 # Analysis Steps (Chain of Thought)
                 1. Calculate the 'Attack Strength' of Home Team vs 'Defense Strength' of Away Team.
                 2. Calculate the 'Attack Strength' of Away Team vs 'Defense Strength' of Home Team.
                 3. Consider Home Field Advantage (usually +10% to +15% scoring probability for home).
                 4. Look at Head-to-Head trends: Do they usually play high-scoring games?

                 # Constraint
                 - Do NOT default to 0-0 or 1-1 unless the stats strictly indicate a defensive deadlock.
                 - Be bold in your prediction if one team has a significant statistical advantage.

                 # Output Format (JSON Only)
                 {
                     "homeTeamScore": [Integer],
                     "awayTeamScore": [Integer]
                 }
                 """;
    }
}
