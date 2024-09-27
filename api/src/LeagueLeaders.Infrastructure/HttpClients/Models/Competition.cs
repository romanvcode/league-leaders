namespace LeagueLeaders.Infrastructure.HttpClients.Models;
public class Competition
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Category Category { get; set; }
}

public class Category
{
    public string Name { get; set; }
}
