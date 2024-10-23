using HotDeskBooking.Commands.Booking.Create;
using HotDeskBooking.Commands.Booking.Delete;
using HotDeskBooking.Commands.Booking.Update;
using HotDeskBooking.Models.Dto;
using HotDeskBooking.Models.Responses;
using HotDeskBooking.Queries.Booking.GetBookingById;
using HotDeskBooking.Queries.Booking.GetFilteredBooking;

namespace HotDeskBooking.Services;

public interface IBookingService
{
    Task<IReadOnlyList<BookingDto>> GetFilteredBookingsAsync(GetFilteredBookingsQuery query, CancellationToken ct);
    Task<BookingDto> GetByIdAsync(GetBookingByIdQuery query, CancellationToken ct);
    Task<CreateOrUpdateBookingResponse> CreateAsync(CreateBookingCommand command, CancellationToken ct);
    Task<CreateOrUpdateBookingResponse> UpdateAsync(UpdateBookingCommand command, CancellationToken ct);
    Task<StandardResponse> DeleteAsync(DeleteBookingCommand command, CancellationToken ct);
}
