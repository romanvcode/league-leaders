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
            Can you predict upcoming ucl matches if i will send you some statistics
            that i have for each team and you will try to analyze and provide me response 
            in desired format:

            # Desired response

            {
                "homeTeamScore": "Int number predicted socer of first(home) team"
                "awayTeamScore": "Int number predicted score of second(away) team"
            }

            # Content

            - Home Team Goals Scored Avg: ${{homeTeamGoalsScoredAvg}}
            - Home Team Goals Conceded Avg: ${{homeTeamGoalsConcededAvg}}
            - Away Team Goals Scored Avg: ${{awayTeamGoalsScoredAvg}}
            - Away Team Goals Conceded Avg: ${{awayTeamGoalsConcededAvg}}
            - Head to Head Results: ${{headToHeadResults}} 
            """;
    }
}
