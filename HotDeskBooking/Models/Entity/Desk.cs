using System.ComponentModel.DataAnnotations.Schema;

namespace HotDeskBooking.Models.Entity;

public class Desk : TrackableEntity
{
    public int Id { get; set; }

    [ForeignKey("Location")]
    public int LocationId { get; set; }
    public bool IsDeleted { get; set; }

    public virtual Location Location { get; set; }
    public virtual List<Booking> Booking { get; set; }
}