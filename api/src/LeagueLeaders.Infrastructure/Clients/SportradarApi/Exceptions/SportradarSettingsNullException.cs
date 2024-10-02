namespace LeagueLeaders.Infrastructure.Clients.SportradarApi.Exceptions;

public class SportradarSettingsNullException : Exception
{
    public SportradarSettingsNullException()
    {
    }

    public SportradarSettingsNullException(string message) : base(message)
    {
    }

    public SportradarSettingsNullException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
