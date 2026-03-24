namespace OidcServer.Models
{
    public class TokenIssuingOptions
    {
        public string Issuer { get; set; } = string.Empty; // this server's Issuer ID, must be an HTTPS URL
        public int IdTokenExpirySeconds { get; set; } = 60 * 40;
        public int AccessTokenExpirySeconds { get; set; } = 60 * 30;
        public int RefreshTokenExpirySeconds { get; set; } = 60 * 24 * 7;
    }
}
        