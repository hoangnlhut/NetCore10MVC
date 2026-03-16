namespace OidcServer.Models
{
    public class CodeItem
    {
        public required AuthenticationRequestModel AuthenRequestModel { get; set; }
        public required string[] Scopes { get; set; }
        public required string User { get; set; }
    }
}
