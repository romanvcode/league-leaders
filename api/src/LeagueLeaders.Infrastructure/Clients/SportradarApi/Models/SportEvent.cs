namespace LeagueLeaders.Infrastructure.Clients.SportradarApi.Models;

public class SportEvent
{
    public string Id { get; set; }
    public int StageId { get; set; }
    public string HomeCompetitorId { get; set; }
    public string AwayCompetitorId { get; set; }
    public string Date { get; set; }
    public string VenueId { get; set; }
    public string RefereeId { get; set; }
    public int HomeCompetitorScore { get; set; }
    public int AwayCompetitorScore { get; set; }
}
