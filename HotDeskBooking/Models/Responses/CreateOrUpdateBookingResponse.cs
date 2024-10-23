using HotDeskBooking.Models.Dto;

namespace HotDeskBooking.Models.Responses;

public record CreateOrUpdateBookingResponse : StandardResponse
{
    public BookingDto? Booking { get; set; }
}
