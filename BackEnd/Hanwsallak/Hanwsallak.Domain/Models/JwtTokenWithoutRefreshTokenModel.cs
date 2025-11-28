namespace Hanwsallak.Domain.Models
{
    public class JwtTokenWithoutRefreshTokenModel
    {
        public required string Token { get; set; }
        public int ExpiryHours { get; set; }
        public DateTime Expiration { get; set; }
    }
}
