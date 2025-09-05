namespace AuthService.Models
{
    public class RegisterVm
    {
        public string  Email { get; set; }
        public string  Password { get; set; }

        public string ConfirmPassword { get; set; }

        // relation with logic of parking
        public string? CustomerNumber { get; set; }
        public int? ParkingContractId { get; set; }
    }
}
