using HotDeskBooking.Models.Dto;

namespace HotDeskBooking.Models.Responses;

public record CreateUserResponse: StandardResponse
{
    public UserDto? User { get; set; }
}
