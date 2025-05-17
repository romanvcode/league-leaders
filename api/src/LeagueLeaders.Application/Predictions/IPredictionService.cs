using LeagueLeaders.Domain;

namespace LeagueLeaders.Application.Predictions;

public interface IPredictionService
{
    Task<Prediction> CreatePredictionAsync(int matchId, int homeTeamScore, int awayTeamScore, bool predicted);
    Task<List<Prediction>> GetPredictionsAsync();
    Task DeletePrediciotnAsync(int predictionId);
}
