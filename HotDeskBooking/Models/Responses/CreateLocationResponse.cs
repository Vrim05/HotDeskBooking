using HotDeskBooking.Models.Dto;

namespace HotDeskBooking.Models.Responses;

public record CreateLocationResponse : StandardResponse
{
    public LocationDto? Location { get; set; }
}
