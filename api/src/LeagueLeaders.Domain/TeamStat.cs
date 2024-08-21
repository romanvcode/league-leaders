namespace LeagueLeaders.Domain
{
    public class TeamStat
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public int RedCards { get; set; }
        public int YellowCards { get; set; }
        public int Substitutions { get; set; }

        public Team Team { get; set; } = null!;
    }
}
