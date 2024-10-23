namespace HotDeskBooking.Models.Responses;

public record StandardResponse
{
    public bool Success { get; set; }
    public string? Error { get; set; }
}