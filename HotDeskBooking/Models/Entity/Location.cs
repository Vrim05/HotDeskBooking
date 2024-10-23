namespace HotDeskBooking.Models.Entity;

public class Location : TrackableEntity
{
    public int Id { get; set; }
    public short BuildNumber { get; set; }
    public short RoomNumber { get; set; }
    public string Name { get; set; } = null!;
    public bool IsDeleted { get; set; }

    public virtual List<Desk> Desks { get; set; }
}