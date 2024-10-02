namespace LeagueLeaders.Infrastructure.Clients.SportradarApi.Models;

public class PlayerStats
{
    public string PlayerId { get; set; }
    public string SportEventId { get; set; }
    public int GoalsScored { get; set; }
    public int Assists { get; set; }
    public int RedCards { get; set; }
    public int YellowCards { get; set; }
    public int ShotsTotal { get; set; }
    public int ShotsOnTarget { get; set; }
}
