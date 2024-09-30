namespace LeagueLeaders.Infrastructure.Clients.SportradarApi.Models;

public class Player
{
    public string Id { get; set; }
    public string CompetitorId { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public int JerseyNumber { get; set; }
    public int Height { get; set; }
    public string Nationality { get; set; }
    public string DateOfBirth { get; set; }
}
