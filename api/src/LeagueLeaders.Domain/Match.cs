namespace LeagueLeaders.Domain;

public class Match
{
    public int Id { get; set; }
    public int StageId { get; set; }
    public int HomeTeamId { get; set; }
    public int AwayTeamId { get; set; }
    public DateTime Date { get; set; }
    public int VenueId { get; set; }
    public int RefereeId { get; set; }
    public int HomeTeamScore { get; set; }
    public int AwayTeamScore { get; set; }

    public Stage Stage { get; set; } = null!;
    public Team HomeTeam { get; set; } = null!;
    public Team AwayTeam { get; set; } = null!;
    public Venue Venue { get; set; } = null!;
    public Referee Referee { get; set; } = null!;
}
