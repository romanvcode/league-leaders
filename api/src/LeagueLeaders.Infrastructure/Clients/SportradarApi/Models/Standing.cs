namespace LeagueLeaders.Infrastructure.Clients.SportradarApi.Models;

public class Standing
{
    public int StageId { get; set; }
    public string CompetitorId { get; set; }
    public int Points { get; set; }
    public int Rank { get; set; }
    public int Played { get; set; }
    public int Win {  get; set; }
    public int Draw {  get; set; }
    public int Loss { get; set; }
    public int GoalsFor { get; set; }
    public int GoalsAgainst { get; set; }
}
