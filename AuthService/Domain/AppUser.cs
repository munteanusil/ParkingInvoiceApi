using Microsoft.AspNetCore.Identity;

namespace AuthService.Domain
{
    public class AppUser : IdentityUser
    {
        public string? CustomerName { get; set; }
        public int? ParkingContratId { get; set; }
    }
}
