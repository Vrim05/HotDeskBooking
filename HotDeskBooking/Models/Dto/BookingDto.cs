namespace HotDeskBooking.Models.Dto;

public record BookingDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int DeskId { get; set; }
    public DateTime StartDay { get; set; }
    public DateTime EndDate { get; set; }

    public virtual UserDto User { get; set; }
    public virtual DeskDto Desk { get; set; }
}