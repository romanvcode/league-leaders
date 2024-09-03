namespace LeagueLeaders.Application.Teams;

public class TeamNotFoundException : Exception
{
    public TeamNotFoundException() { }

    public TeamNotFoundException(string message)
        : base(message) { }

    public TeamNotFoundException(string message, Exception innerException)
        : base(message, innerException) { }
}
