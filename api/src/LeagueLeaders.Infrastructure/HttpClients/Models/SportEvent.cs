namespace LeagueLeaders.Infrastructure.HttpClients.Models;
public class SportEvent
{
    public string Id { get; set; }
    public SportEventContext SportEventContext { get; set; }
    public List<Competitor> Competitors { get; set; }
}

public class SportEventContext
{
    public Stage Stage { get; set; }
}