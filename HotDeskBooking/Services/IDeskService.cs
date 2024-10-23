using HotDeskBooking.Commands.Booking.Delete;
using HotDeskBooking.Commands.Desks.Create;
using HotDeskBooking.Commands.Desks.Delete;
using HotDeskBooking.Models.Dto;
using HotDeskBooking.Models.Responses;
using HotDeskBooking.Queries.Desks.GetDeskById;
using HotDeskBooking.Queries.Desks.GetFilteredDesk;

namespace HotDeskBooking.Services;

public interface IDeskService
{
    Task<GetFilteredDesksResponse> GetFilteredDesksAsync(GetFilteredDesksQuery query, CancellationToken ct);
    Task<DeskDto> GetByIdAsync(GetDeskByIdQuery query, CancellationToken ct);
    Task<CreateDeskResponse> CreateAsync(CreateDeskCommand command, CancellationToken ct);
    Task<StandardResponse> DeleteAsync(DeleteDeskCommand command, CancellationToken ct);
}
