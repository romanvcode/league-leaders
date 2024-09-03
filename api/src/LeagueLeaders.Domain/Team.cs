namespace LeagueLeaders.Domain;

public class Team
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Abbreviation { get; set; }
    public string? Country { get; set; }
    public string? Stadium { get; set; }
    public string? Manager { get; set; }

    public List<Player> Players { get; set; } = new List<Player>();
}
