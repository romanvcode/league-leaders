using FluentValidation;

namespace LeagueLeaders.API.Validators;

public class MatchIdValidor : AbstractValidator<int>
{
    public MatchIdValidor()
    {
        RuleFor(x => x)
            .GreaterThan(0)
            .WithMessage("Match ID must be greater than 0.");
    }
}
