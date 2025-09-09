using Microsoft.AspNetCore.Identity;

namespace AuthService.Domain
{
    public class AppUser : IdentityUser
    {
        public string? CustomerName { get; set; }
        public string? CustomerNumber { get; internal set; }
        public string? ParkingContractId { get; internal set; }
    }
}
