namespace LeagueLeaders.Application.Leaderboard;

public class StandingsNotFoundException : Exception
{
    public StandingsNotFoundException() { }

    public StandingsNotFoundException(string message)
        : base(message) { }

    public StandingsNotFoundException(string message, Exception innerException)
        : base(message, innerException) { }
}
