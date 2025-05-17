using LeagueLeaders.Domain;
using LeagueLeaders.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;

namespace LeagueLeaders.Application.Predictions;

public class PredictionService : IPredictionService
{
    private readonly LeagueLeadersDbContext _context;
    private readonly IChatClient _chatClient;

    public PredictionService(LeagueLeadersDbContext context, IChatClient chatClient)
    {
        _context = context;
        _chatClient = chatClient;
    }

    public async Task<Prediction> CreatePredictionAsync(int matchId, int homeTeamScore, int awayTeamScore, bool predicted)
    {
        var match = await _context.Matches
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == matchId)
            ?? throw new MatchesNotFoundException($"Match with Id {matchId} not found.");

        if (match.Date < DateTime.UtcNow)
            throw new MatchAlreadyStartedException($"Match with Id {matchId} already started.");

        var predictionForMatchExists = await _context.Predictions
            .AsNoTracking()
            .AnyAsync(p => p.MatchId == matchId);

        if (predictionForMatchExists)
            throw new PredictionAlreadyExistsException($"Prediction for match with Id {matchId} already exists.");

        if (!predicted)
            (homeTeamScore, awayTeamScore) = await PredictScore(match.HomeTeamId, match.AwayTeamId);

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
            .Include(p => p.Match)
                .ThenInclude(m => m.HomeTeam)
            .Include(p => p.Match)
                .ThenInclude(m => m.AwayTeam)
            .ToListAsync();

        foreach (var prediction in predictions)
        {
            var match = await _context.Matches
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == prediction.MatchId);

            if (match == null || match.Date > DateTime.UtcNow)
                continue;

            prediction.IsCorrect = match.HomeTeamScore == prediction.HomeTeamScore && match.AwayTeamScore == prediction.AwayTeamScore;
        }

        return predictions;
    }

    public async Task DeletePrediciotnAsync(int predictionId)
    {
        var prediction = await _context.Predictions
            .FirstOrDefaultAsync(p => p.Id == predictionId)
            ?? throw new PredictionNotFoundException($"Prediction with Id {predictionId} not found.");

        _context.Predictions.Remove(prediction);
        await _context.SaveChangesAsync();
    }

    private async Task<(int homeTeamScore, int awayTeamScore)> PredictScore(int homeTeamId, int awayTeamId)
    {
        var homeMatches = await _context.Matches
            .Where(m => m.HomeTeamId == homeTeamId || m.AwayTeamId == homeTeamId)
            .ToListAsync();

        var awayMatches = await _context.Matches
            .Where(m => m.HomeTeamId == awayTeamId || m.AwayTeamId == awayTeamId)
            .ToListAsync();

        var matchesPlayedByHomeTeam = homeMatches.Count;
        var matchesPlayedByAwayTeam = awayMatches.Count;

        if (matchesPlayedByHomeTeam == 0 || matchesPlayedByAwayTeam == 0)
            return (0, 0);

        var homeTeamGoalsScoredAvg = homeMatches.TotalGoalsScoredBy(homeTeamId) / (double)matchesPlayedByHomeTeam;
        var homeTeamGoalsConcededAvg = homeMatches.TotalGoalsConcededBy(homeTeamId) / (double)matchesPlayedByHomeTeam;

        var awayTeamGoalsScoredAvg = awayMatches.TotalGoalsScoredBy(awayTeamId) / (double)matchesPlayedByAwayTeam;
        var awayTeamGoalsConcededAvg = awayMatches.TotalGoalsConcededBy(awayTeamId) / (double)matchesPlayedByAwayTeam;

        var headToHeadResults = homeMatches
            .Where(m => m.HomeTeamId == awayTeamId || m.AwayTeamId == awayTeamId)
            .Select(m => (
                (m.HomeTeamId == homeTeamId ? ChatMessages.HomeTeam : ChatMessages.AwayTeam, m.HomeTeamScore), 
                (m.AwayTeamId == homeTeamId ? ChatMessages.HomeTeam : ChatMessages.AwayTeam, m.AwayTeamScore))).ToList();

        var chatPrediciton = await _chatClient
            .GetResponseAsync<ChatPrediction>(ChatMessages.GetPredictionRequest(
                homeTeamGoalsScoredAvg,
                homeTeamGoalsConcededAvg,
                awayTeamGoalsScoredAvg,
                awayTeamGoalsConcededAvg,
                headToHeadResults
            ));

        return (chatPrediciton.Result.homeTeamScore, chatPrediciton.Result.awayTeamScore);
    }
}

public record ChatPrediction(int homeTeamScore, int awayTeamScore);
