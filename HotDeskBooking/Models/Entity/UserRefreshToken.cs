using System.ComponentModel.DataAnnotations.Schema;

namespace HotDeskBooking.Models.Entity;

public class UserRefreshToken
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int UserId { get; set; }
    public string RefreshTokenHash { get; set; }
    public DateTime RefreshTokenExpires { get; set; }
}
