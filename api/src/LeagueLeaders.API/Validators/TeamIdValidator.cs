using FluentValidation;

namespace LeagueLeaders.API.Validators;

public class TeamIdValidator : AbstractValidator<int>
{
    public TeamIdValidator()
    {
        RuleFor(x => x)
            .GreaterThan(0)
            .WithMessage("Team ID must be greater than 0.");
    }
}
