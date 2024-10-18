using LeagueLeaders.Application.Predictions;
using LeagueLeaders.Domain;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using LeagueLeaders.API.Validators;

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

        var createdPrediction = await _predictionService.CreatePredictionAsync(matchId, homeTeamScore, awayTeamScore);

        return createdPrediction;
    }

    [HttpGet]
    public async Task<List<Prediction>> GetPredictionsAsync()
    {
        var predicitons = await _predictionService.GetPredictionsAsync();

        return predicitons;
    }
}
