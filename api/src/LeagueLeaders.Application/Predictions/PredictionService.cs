using LeagueLeaders.Domain;
using LeagueLeaders.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace LeagueLeaders.Application.Predictions;

public class PredictionService : IPredictionService
{
    private readonly LeagueLeadersDbContext _context;

    public PredictionService(LeagueLeadersDbContext context)
    {
        _context = context;
    }

    public async Task<Prediction> CreatePredictionAsync(int matchId, int homeTeamScore, int awayTeamScore)
    {
        var match = await _context.Matches
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == matchId)
            ?? throw new MatchesNotFoundException($"Match with Id {matchId} not found.");

        var predictionForMatchExists = await _context.Predictions
            .AsNoTracking()
            .AnyAsync(p => p.MatchId == matchId);

        if (predictionForMatchExists)
            throw new PredictionAlreadyExistsException($"Prediction for match with Id {matchId} already exists.");

        var prediction = new Prediction
        {
            MatchId = matchId,
            HomeTeamScore = homeTeamScore,
            AwayTeamScore = awayTeamScore
        };
        
        _context.Predictions.Add(prediction);
        await _context.SaveChangesAsync();

        return prediction;
    }

    public async Task<List<Prediction>> GetPredictionsAsync()
    {
        var predictions = await _context.Predictions
            .AsNoTracking()
            .ToListAsync();

        return predictions;
    }
}
