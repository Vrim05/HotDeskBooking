using System.ComponentModel.DataAnnotations.Schema;

namespace HotDeskBooking.Models.Entity;

public class User : TrackableEntity
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string? PasswordHash { get; set; } = null!;
    [ForeignKey("Role")]
    public int RoleId { get; set; }

    public virtual UserRole Role { get; set; }
    public virtual List<Booking> Bookings { get; set; }
}