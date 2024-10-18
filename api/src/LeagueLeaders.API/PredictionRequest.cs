namespace LeagueLeaders.API
{
    public class PredictionRequest
    {
        public int MatchId { get; set; }
        public int HomeTeamScore { get; set; }
        public int AwayTeamScore { get; set; }
    }
}
