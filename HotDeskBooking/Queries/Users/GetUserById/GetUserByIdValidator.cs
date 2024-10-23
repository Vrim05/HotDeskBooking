using FluentValidation;

namespace HotDeskBooking.Queries.Users.GetUserById;

public class GetUserByIdValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
