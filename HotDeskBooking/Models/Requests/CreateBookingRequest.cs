namespace HotDeskBooking.Models.Requests;

public record CreateBookingRequest
{
    public int UserId { get; init; }
    public int DeskId { get; init; }
    public DateTime StartDay { get; init; }
    public DateTime EndDate { get; init; }
}
