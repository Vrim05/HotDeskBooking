using HotDeskBooking.Models.Responses;
using MediatR;

namespace HotDeskBooking.Queries.Desks.GetFilteredDesk;

public record GetFilteredDesksQuery : IRequest<GetFilteredDesksResponse>
{
    public int? LocationId { get; init; }
    public DateTime? StartDay { get; init; }
    public DateTime? EndDate { get; init; }
}
