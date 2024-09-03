namespace LeagueLeaders.Domain;

public class Stage
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int SeasonId { get; set; }
    public string? Type { get; set; }

    public List<Match> Matches { get; set; } = new List<Match>();
    public Season Season { get; set; } = null!;
}
