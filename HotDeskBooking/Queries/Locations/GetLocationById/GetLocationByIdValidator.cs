using FluentValidation;

namespace HotDeskBooking.Queries.Locations.GetLocationById;

public class GetLocationByIdValidator : AbstractValidator<GetLocationByIdQuery>
{
    public GetLocationByIdValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
