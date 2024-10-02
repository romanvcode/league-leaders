namespace LeagueLeaders.Infrastructure.Clients.SportradarApi.Models;

public class CompetitorStats
{
    public string TeamId { get; set; }
    public string SportEventId { get; set; }
    public int Possession { get; set; }
    public int RedCards { get; set; }
    public int YellowCards { get; set; }
    public int CornerKicks { get; set; }
    public int Offsides { get; set; }
    public int Fouls { get; set; }
    public int ShotsTotal { get; set; }
    public int ShotsOnTarget { get; set; }
}
