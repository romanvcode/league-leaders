namespace LeagueLeaders.Application.Predictions;

public class PredictionNotFoundException : Exception
{
    public PredictionNotFoundException()
    {
    }

    public PredictionNotFoundException(string message) : base(message)
    {
    }

    public PredictionNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
