namespace LeagueLeaders.Domain
{
    public class Season
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CompetitionId { get; set; }
        public DateOnly? StartAt { get; set; }
        public DateOnly? EndAt { get; set; }

        public List<Stage> Stages { get; set; } = new List<Stage>();
        public Competition Competition { get; set; } = null!;
    }
}
