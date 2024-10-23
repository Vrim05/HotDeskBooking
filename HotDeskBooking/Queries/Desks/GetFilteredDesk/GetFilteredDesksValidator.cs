using FluentValidation;
using HotDeskBooking.Queries.Desks.GetDeskById;

namespace HotDeskBooking.Queries.Desks.GetFilteredDesk;

public class GetFilteredDesksValidator : AbstractValidator<GetFilteredDesksQuery>
{
    public GetFilteredDesksValidator()
    {
        RuleFor(x => x.EndDate)
            .NotNull()
            .When(x => x.StartDay.HasValue)
            .WithMessage("EndDate can only be provided if StartDay is also provided.");

        RuleFor(x => x.StartDay)
            .NotNull()
            .When(x => x.EndDate.HasValue)
            .WithMessage("StartDay must be provided if EndDate is set.");

        RuleFor(x => x)
            .Must(x => !x.StartDay.HasValue || !x.EndDate.HasValue || x.EndDate >= x.StartDay)
            .WithMessage("EndDate cannot be earlier than StartDay.");
    }
}
