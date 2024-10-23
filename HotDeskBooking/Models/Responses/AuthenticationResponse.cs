using HotDeskBooking.Models.Dto;

namespace HotDeskBooking.Models.Responses;

public record AuthenticationResponse : StandardResponse
{
    public string? Token { get; set; }
    public DateTime? TokenExpires { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpires { get; set; }
    public UserRoleDto? UserRole { get; set; }
    public int UserId { get; set; }
}
