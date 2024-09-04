namespace LeagueLeaders.Domain;

public class TeamStat
{
    public int Id { get; set; }
    public int MatchId { get; set; }
    public int TeamId { get; set; }
    public int Possession { get; set; }
    public int RedCards { get; set; }
    public int YellowCards { get; set; }
    public int Corners { get; set; }
    public int Offsides { get; set; }
    public int Fouls { get; set; }
    public int Shots { get; set; }
    public int ShotsOnTarget { get; set; }

    public Match Match { get; set; } = null!;
    public Team Team { get; set; } = null!;
}
