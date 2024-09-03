namespace LeagueLeaders.Application.Teams;

public class PlayersNotFoundException : Exception
{
    public PlayersNotFoundException() { }

    public PlayersNotFoundException(string message)
        : base(message) { }

    public PlayersNotFoundException(string message, Exception innerException)
        : base(message, innerException) { }
}
