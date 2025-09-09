namespace AuthService.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string UserId { get; set; } = default!;
        public string TokenHash { get; set; } = default!;
        public DateTime ExpiresUtc { get; set; }
        public DateTime? RevokedUtc { get; set; }
    }
}
