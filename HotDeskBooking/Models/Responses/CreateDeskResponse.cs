using HotDeskBooking.Models.Dto;

namespace HotDeskBooking.Models.Responses;

public record CreateDeskResponse : StandardResponse
{
    public DeskDto? Desk { get; set; }
}
