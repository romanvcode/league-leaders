using LeagueLeaders.Application.Predictions;
using LeagueLeaders.Domain;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using LeagueLeaders.API.Validators;

namespace LeagueLeaders.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PredictionController : ControllerBase
{
    private readonly IPredictionService _predictionService;

    public PredictionController(IPredictionService predictionService)
    {
        _predictionService = predictionService;
    }

    [HttpPost]
    public async Task<Prediction> CreatePredictionAsync(int matchId, int homeTeamScore, int awayTeamScore)
    {
        var validator = new MatchIdValidor();
        var validationResult = validator.Validate(matchId);

        if (!validationResult.IsValid) {
            throw new ValidationException(validationResult.Errors);
        }

        var scoreValidator = new ScoreValidator();
        var homeScoreValidationResult = scoreValidator.Validate(homeTeamScore);
        var awayScoreValidationResult = scoreValidator.Validate(awayTeamScore);

        if (!homeScoreValidationResult.IsValid || !awayScoreValidationResult.IsValid)
        {
            throw new ValidationException(homeScoreValidationResult.Errors);
        }

        var prediction = await _predictionService.CreatePredictionAsync(matchId, homeTeamScore, awayTeamScore);

        return prediction;
    }

    [HttpGet]
    public async Task<List<Prediction>> GetPredictionsAsync()
    {
        var predicitons = await _predictionService.GetPredictionsAsync();

        return predicitons;
    }
}
