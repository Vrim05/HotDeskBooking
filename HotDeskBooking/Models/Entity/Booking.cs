using System.ComponentModel.DataAnnotations.Schema;

namespace HotDeskBooking.Models.Entity;

public class Booking : TrackableEntity
{
    public int Id { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }

    [ForeignKey("Desk")]
    public int DeskId { get; set; }
    public DateTime StartDay { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsDeleted { get; set; }

    public virtual User User { get; set; }
    public virtual Desk Desk { get; set; }

}