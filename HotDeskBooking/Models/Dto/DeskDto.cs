namespace HotDeskBooking.Models.Dto;

public record DeskDto
{
    public int Id { get; set; }
    public int LocationId { get; set; }

    public virtual LocationDto Location { get; set; }
}