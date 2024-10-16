namespace LeagueLeaders.Application.Predictions;

public class PredictionAlreadyExistsException : Exception
{
    public PredictionAlreadyExistsException()
    {
    }

    public PredictionAlreadyExistsException(string message) : base(message)
    {
    }

    public PredictionAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
