namespace LeagueLeaders.Domain
{
    public class Competition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Region { get; set; }

        public List<Season> Seasons { get; set; } = new List<Season>();
    }
}
