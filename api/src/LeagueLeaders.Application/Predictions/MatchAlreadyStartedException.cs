namespace LeagueLeaders.Application.Predictions;

public class MatchAlreadyStartedException : Exception
{
    public MatchAlreadyStartedException()
    {
    }
    public MatchAlreadyStartedException(string message) : base(message)
    {
    }

    public MatchAlreadyStartedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
