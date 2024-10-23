namespace HotDeskBooking.Models.Requests;

public record CreateLocationRequest
{
    public short BuildNumber { get; init; }
    public short RoomNumber { get; init; }
    public string Name { get; init; } = null!;
}
