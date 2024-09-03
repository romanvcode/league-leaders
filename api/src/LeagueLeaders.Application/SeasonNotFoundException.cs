namespace LeagueLeaders.Application;

public class SeasonNotFoundException : Exception
{
    public SeasonNotFoundException() { }

    public SeasonNotFoundException(string message)
        : base(message) { }

    public SeasonNotFoundException(string message, Exception innerException)
        : base(message, innerException) { }
}
