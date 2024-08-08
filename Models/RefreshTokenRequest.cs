namespace FacturasAPI.Models
{
    public class RefreshTokenRequest
    {
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
