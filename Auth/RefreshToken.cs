namespace MigraziiRabotaitePj.Auth
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpireAtUtc { get; set; }
        public bool IsRevoke { get; set; }
        public Guid UserId { get; set; }
        public AppUser User { get; set; }
    }
}