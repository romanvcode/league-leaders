using LeagueLeaders.Domain;

namespace LeagueLeaders.Application.Predictions;

public interface IPredictionService
{
    Task<Prediction> CreatePredictionAsync(int matchId, int homeTeamScore, int awayTeamScore);
    Task<List<Prediction>> GetPredictionsAsync();
}
