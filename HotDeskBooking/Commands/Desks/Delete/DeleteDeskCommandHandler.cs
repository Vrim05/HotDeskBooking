using HotDeskBooking.Models.Responses;
using HotDeskBooking.Services;
using MediatR;


namespace HotDeskBooking.Commands.Desks.Delete;

public class DeleteDeskCommandHandler : IRequestHandler<DeleteDeskCommand, StandardResponse>
{
    public readonly IDeskService _deskService;
    public DeleteDeskCommandHandler(IDeskService deskService)
    {
        _deskService = deskService;
    }

    public async Task<StandardResponse> Handle(DeleteDeskCommand request, CancellationToken ct)
        => await _deskService.DeleteAsync(request, ct);
}
