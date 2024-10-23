using HotDeskBooking.Models.Responses;
using HotDeskBooking.Services;
using MediatR;

namespace HotDeskBooking.Queries.Desks.GetFilteredDesk;

public class GetFilteredDesksQueryHandler : IRequestHandler<GetFilteredDesksQuery, GetFilteredDesksResponse>
{
    public readonly IDeskService _deskService;
    public GetFilteredDesksQueryHandler(IDeskService deskService)
    {
        _deskService = deskService;
    }

    public async Task<GetFilteredDesksResponse> Handle(GetFilteredDesksQuery query, CancellationToken ct)
        => await _deskService.GetFilteredDesksAsync(query, ct);
}
