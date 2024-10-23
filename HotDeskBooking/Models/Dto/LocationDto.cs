namespace HotDeskBooking.Models.Dto;

public record LocationDto
{
    public int Id { get; set; }
    public short BuildNumber { get; set; }
    public short RoomNumber { get; set; }
    public string Name { get; set; } = null!;
}