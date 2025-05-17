using FluentValidation;

namespace LeagueLeaders.API.Validators;

public class MatchIdValidator : AbstractValidator<int>
{
    public MatchIdValidator()
    {
        RuleFor(x => x)
            .GreaterThan(0)
            .WithMessage("Match ID must be greater than 0.");
    }
}
