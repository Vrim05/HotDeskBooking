using FluentValidation;
using MediatR;

namespace HotDeskBooking.Helpers;

public class ValidatorPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidatorPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
      => _validators = validators;

    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var failure = _validators
            .Select(v => v.Validate(request))
            .FirstOrDefault();

        if (failure is not null && !failure.IsValid)
            throw new ValidationException(failure.Errors);

        return next();
    }
}
