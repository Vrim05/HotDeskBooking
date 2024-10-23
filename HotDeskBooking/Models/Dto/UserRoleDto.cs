namespace HotDeskBooking.Models.Dto;

public record UserRoleDto
{
    public UserRoleDto() {}
    public UserRoleDto(string roleName, int id)
    {
        Type = roleName;
        Id = id;
    }
    public int Id { get; set; }
    public string Type { get; set; }
}