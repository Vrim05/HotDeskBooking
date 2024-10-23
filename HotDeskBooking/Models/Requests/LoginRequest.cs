namespace HotDeskBooking.Models.Requests;

public record LoginRequest
{
    public string Email { get; init; }
    public string Password { get; init; }
}
