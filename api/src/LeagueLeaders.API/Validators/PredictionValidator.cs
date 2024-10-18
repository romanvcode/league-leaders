using FluentValidation;

namespace LeagueLeaders.API.Validators;

public class PredictionValidator : AbstractValidator<PredictionRequest>
{
    public PredictionValidator()
    {
        RuleFor(x => x.MatchId)
            .GreaterThan(0)
            .WithMessage("Match ID must be greater than 0.");
        RuleFor(x => x.HomeTeamScore)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Score must be greater than or equal to 0.");
        RuleFor(x => x.AwayTeamScore)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Score must be greater than or equal to 0.");
    }
}
