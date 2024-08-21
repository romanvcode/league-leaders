namespace LeagueLeaders.Domain
{
    public class PlayerStat
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int TeamId { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public int RedCards { get; set; }
        public int YellowCards { get; set; }

        public Player Player { get; set; } = null!;
        public Team Team { get; set; } = null!;
    }
}
