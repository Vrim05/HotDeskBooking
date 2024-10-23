namespace HotDeskBooking.Models.Dto;

public record UserDto
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public int RoleId { get; set; }

    public virtual UserRoleDto Role { get; set; }
}