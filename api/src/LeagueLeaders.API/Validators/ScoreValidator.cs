using FluentValidation;

namespace LeagueLeaders.API.Validators;

public class ScoreValidator : AbstractValidator<int>
{
    public ScoreValidator()
    {
        RuleFor(s => s)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Score must be greater than or equal to 0.");
    }
}
