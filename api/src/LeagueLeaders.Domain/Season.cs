namespace LeagueLeaders.Domain;

public class Season
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CompetitionId { get; set; }
    public DateTime? StartAt { get; set; }
    public DateTime? EndAt { get; set; }
    public int SportradarId { get; set; }

    public List<Stage> Stages { get; set; } = new List<Stage>();
    public Competition Competition { get; set; } = null!;
}
