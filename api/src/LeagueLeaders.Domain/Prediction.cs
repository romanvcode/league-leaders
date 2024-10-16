namespace LeagueLeaders.Domain;

public class Prediction
{
    public int Id { get; set; }
    public int HomeTeamScore { get; set; }
    public int AwayTeamScore { get; set; }
    public int MatchId { get; set; }

    public Match Match { get; set; } = null!;
}
