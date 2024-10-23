using HotDeskBooking.Models.Dto;

namespace HotDeskBooking.Models.Responses;

public record GetFilteredDesksResponse : StandardResponse
{
    public IReadOnlyList<DeskDto>? AvailableDesks { get; set; }
    public IReadOnlyList<DeskDto>? UnavailableDesks { get; set; }
}
