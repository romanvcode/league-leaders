using LeagueLeaders.Application.Predictions;
using LeagueLeaders.Domain;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using LeagueLeaders.API.Validators;
using LeagueLeaders.API.Workers;

namespace LeagueLeaders.API.Controllers;
[Route("api/predictions")]
[ApiController]
public class PredictionController : ControllerBase
{
    private readonly IPredictionService _predictionService;

    public PredictionController(IPredictionService predictionService)
    {
        _predictionService = predictionService;
    }

    [HttpPost]
    public async Task<Prediction> CreatePredictionAsync(PredictionRequest predictionRequest)
    {
        var predictionValidator = new PredictionValidator();
        await predictionValidator.ValidateAndThrowAsync(predictionRequest);

        var matchId = predictionRequest.MatchId;
        var homeTeamScore = predictionRequest.HomeTeamScore;
        var awayTeamScore = predictionRequest.AwayTeamScore;
        var predicted = predictionRequest.Predicted;

        var createdPrediction = await _predictionService.CreatePredictionAsync(matchId, homeTeamScore, awayTeamScore, predicted);

        return createdPrediction;
    }

    [HttpGet]
    public async Task<List<Prediction>> GetPredictionsAsync()
    {
        var predicitons = await _predictionService.GetPredictionsAsync();

        return predicitons;
    }

    [HttpDelete("{predictionId}")]
    public async Task DeletePredictionAsync(int predictionId)
    {
        await _predictionService.DeletePrediciotnAsync(predictionId);
    }

    [HttpGet("backtest-csv")]
    public async Task<IActionResult> DownloadBacktestCsv()
    {
        try
        {
            var csvBytes = await _predictionService.GenerateBacktestCsvAsync();
            var fileName = $"prediction_analysis_{DateTime.UtcNow:yyyyMMdd_HHmm}.csv";

            return File(csvBytes, "text/csv", fileName);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Analysis failed: {ex.Message}");
        }
    }
}
