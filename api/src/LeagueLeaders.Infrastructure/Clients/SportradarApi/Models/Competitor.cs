namespace LeagueLeaders.Infrastructure.Clients.SportradarApi.Models;

public class Competitor
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Abbreviation { get; set; }
    public string Country { get; set; } = "N/A";
    public string Stadium { get; set; } = "N/A";
    public string Manager { get; set; } = "N/A";
}
