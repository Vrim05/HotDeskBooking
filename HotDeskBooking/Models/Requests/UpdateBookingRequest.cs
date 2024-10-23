namespace HotDeskBooking.Models.Requests;

public record UpdateBookingRequest
{
    public int Id { get; init; }
    public int NewDeskId { get; init; }
    public DateTime NewStartDay { get; init; }
    public DateTime NewEndDate { get; init; }
}
