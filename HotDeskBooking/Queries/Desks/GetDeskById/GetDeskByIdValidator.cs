using FluentValidation;

namespace HotDeskBooking.Queries.Desks.GetDeskById;

public class GetDeskByIdValidator : AbstractValidator<GetDeskByIdQuery>
{
    public GetDeskByIdValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
