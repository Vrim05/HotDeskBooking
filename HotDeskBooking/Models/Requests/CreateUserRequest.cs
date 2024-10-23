using HotDeskBooking.Models.Dto;

namespace HotDeskBooking.Models.Requests;

public record CreateUserRequest
{
    public string Email { get; init; }
    public string Password { get; init; }
    public int RoleId { get; init; }
}
