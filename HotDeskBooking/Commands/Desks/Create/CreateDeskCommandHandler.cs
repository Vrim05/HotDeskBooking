using HotDeskBooking.Models.Responses;
using HotDeskBooking.Services;
using MediatR;


namespace HotDeskBooking.Commands.Desks.Create;

public class CreateDeskCommandHandler : IRequestHandler<CreateDeskCommand, CreateDeskResponse>
{
    public readonly IDeskService _deskService;
    public CreateDeskCommandHandler(IDeskService deskService)
    {
        _deskService = deskService;
    }

    public async Task<CreateDeskResponse> Handle(CreateDeskCommand request, CancellationToken ct)
        => await _deskService.CreateAsync(request, ct);
}
