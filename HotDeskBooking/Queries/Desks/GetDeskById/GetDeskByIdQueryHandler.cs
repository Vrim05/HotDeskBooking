using HotDeskBooking.Models.Dto;
using HotDeskBooking.Services;
using MediatR;

namespace HotDeskBooking.Queries.Desks.GetDeskById;

public class GetDeskByIdQueryHandler : IRequestHandler<GetDeskByIdQuery, DeskDto>
{
    public readonly IDeskService _deskService;
    public GetDeskByIdQueryHandler(IDeskService deskService)
    {
        _deskService = deskService;
    }

    public async Task<DeskDto> Handle(GetDeskByIdQuery query, CancellationToken ct)
        => await _deskService.GetByIdAsync(query, ct);
}
