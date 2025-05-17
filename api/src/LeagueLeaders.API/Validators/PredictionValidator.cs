using FluentValidation;

namespace LeagueLeaders.API.Validators;

public class PredictionValidator : AbstractValidator<PredictionRequest>
{
    public PredictionValidator()
    {
        RuleFor(x => x.MatchId)
            .GreaterThan(0)
            .WithMessage("Match ID must be greater than 0.");
    }
}
