namespace LeagueLeaders.Infrastructure.Clients.SportradarApi.Exceptions;

public class SportradarBadResponseException : Exception
{
    public SportradarBadResponseException()
    {
    }

    public SportradarBadResponseException(string message) : base(message)
    {
    }

    public SportradarBadResponseException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
