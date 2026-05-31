using LeagueLeaders.Domain;
using LeagueLeaders.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using System.Text;

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

    public async Task<byte[]> GenerateBacktestCsvAsync()
    {
        var csvBuilder = new StringBuilder();

        csvBuilder.AppendLine(
            "MatchId,Date,Home Team,Away Team,Actual Home,Actual Away,Predicted Home,Predicted Away,Result,Exact Match?,Outcome Correct?");

        var pastMatches = await _context.Matches
            .AsNoTracking()
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Where(m => m.Date < DateTime.UtcNow)
            .OrderBy(m => m.Date)
            .ToListAsync();

        int correctExactPredictions = 0;
        int correctOutcomePredictions = 0;
        int totalMatchesAnalyzed = 0;

        foreach (var match in pastMatches)
        {
            var (predHome, predAway) = await PredictScoreWithDateLimit(
                match.HomeTeamId,
                match.AwayTeamId,
                match.Date
            );

            bool isExact = (predHome == match.HomeTeamScore && predAway == match.AwayTeamScore);

            var actualOutcome = GetMatchOutcome(match.HomeTeamScore, match.AwayTeamScore);
            var predOutcome = GetMatchOutcome(predHome, predAway);
            bool isOutcomeCorrect = (actualOutcome == predOutcome);

            totalMatchesAnalyzed++;
            if (isExact) correctExactPredictions++;
            if (isOutcomeCorrect) correctOutcomePredictions++;

            var lineItems = new List<string>
            {
                match.Id.ToString(),
                match.Date.ToString("yyyy-MM-dd"),
                EscapeCsv(match.HomeTeam.Name),
                EscapeCsv(match.AwayTeam.Name),
                match.HomeTeamScore.ToString(),
                match.AwayTeamScore.ToString(),
                predHome.ToString(),
                predAway.ToString(),
                actualOutcome,
                isExact ? "TRUE" : "FALSE",
                isOutcomeCorrect ? "TRUE" : "FALSE"
            };

            csvBuilder.AppendLine(string.Join(",", lineItems));
        }

        double outcomeAccuracy = totalMatchesAnalyzed > 0
            ? Math.Round((double)correctOutcomePredictions / totalMatchesAnalyzed * 100, 2)
            : 0;

        double exactAccuracy = totalMatchesAnalyzed > 0
            ? Math.Round((double)correctExactPredictions / totalMatchesAnalyzed * 100, 2)
            : 0;

        csvBuilder.AppendLine();
        csvBuilder.AppendLine("STATISTICS REPORT,,,,,,,,");
        csvBuilder.AppendLine($"Total Matches,{totalMatchesAnalyzed},,,,,,,,");
        csvBuilder.AppendLine($"Winner Accuracy,{outcomeAccuracy}%,,,,,,,,");
        csvBuilder.AppendLine($"Exact Score Accuracy,{exactAccuracy}%,,,,,,,,");

        return Encoding.UTF8.GetBytes(csvBuilder.ToString());
    }

    private static string EscapeCsv(string field)
    {
        if (field.Contains(',') || field.Contains('"') || field.Contains('\n'))
        {
            return $"\"{field.Replace("\"", "\"\"")}\"";
        }
        return field;
    }

    private async Task<(int homeTeamScore, int awayTeamScore)> PredictScoreWithDateLimit(int homeTeamId, int awayTeamId, DateTime dateLimit)
    {
        var homeMatches = await _context.Matches
            .Where(m => (m.HomeTeamId == homeTeamId || m.AwayTeamId == homeTeamId) && m.Date < dateLimit)
            .Take(5)
            .ToListAsync();

        var awayMatches = await _context.Matches
            .Where(m => (m.HomeTeamId == awayTeamId || m.AwayTeamId == awayTeamId) && m.Date < dateLimit)
            .Take(5)
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
                (m.AwayTeamId == homeTeamId ? ChatMessages.HomeTeam : ChatMessages.AwayTeam, m.AwayTeamScore)))
            .ToList();

        var chatPrediction = await _chatClient
            .GetResponseAsync<ChatPrediction>(ChatMessages.GetPredictionRequest(
                homeTeamGoalsScoredAvg,
                homeTeamGoalsConcededAvg,
                awayTeamGoalsScoredAvg,
                awayTeamGoalsConcededAvg,
                headToHeadResults
            ));

        return (chatPrediction.Result.HomeTeamScore, chatPrediction.Result.AwayTeamScore);
    }

    private string GetMatchOutcome(int homeScore, int awayScore)
    {
        if (homeScore > awayScore) return "HomeWin";
        if (awayScore > homeScore) return "AwayWin";
        return "Draw";
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
                (m.AwayTeamId == homeTeamId ? ChatMessages.HomeTeam : ChatMessages.AwayTeam, m.AwayTeamScore)))
            .ToList();

        var chatPrediction = await _chatClient
            .GetResponseAsync<ChatPrediction>(ChatMessages.GetPredictionRequest(
                homeTeamGoalsScoredAvg,
                homeTeamGoalsConcededAvg,
                awayTeamGoalsScoredAvg,
                awayTeamGoalsConcededAvg,
                headToHeadResults
            ));

        return (chatPrediction.Result.HomeTeamScore, chatPrediction.Result.AwayTeamScore);
    }
}

public record ChatPrediction(int HomeTeamScore, int AwayTeamScore);
