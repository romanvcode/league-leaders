namespace LeagueLeaders.Application.Matches;

public class TeamStatsNotFoundException : Exception
{
    public TeamStatsNotFoundException() { }

    public TeamStatsNotFoundException(string message)
        : base(message) { }

    public TeamStatsNotFoundException(string message, Exception innerException)
        : base(message, innerException) { }
}
