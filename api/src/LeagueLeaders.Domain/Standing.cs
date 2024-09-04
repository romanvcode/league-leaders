namespace LeagueLeaders.Domain;

public class Standing
{
    public int Id { get; set; }
    public int StageId { get; set; }
    public int TeamId { get; set; }
    public int Points { get; set; }
    public int Place { get; set; }
    public int MatchesPlayed { get; set; }
    public int Wins { get; set; }
    public int Draws { get; set; }
    public int Losses { get; set; }
    public int GoalsFor { get; set; }
    public int GoalsAgainst { get; set; }

    public Stage Stage { get; set; } = null!;
    public Team Team { get; set; } = null!;
}
