namespace HotDeskBooking.Models.Requests;

public record CreateDeskRequest
{
    public int LocationId { get; init; }
}
