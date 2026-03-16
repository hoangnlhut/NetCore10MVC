namespace OidcDemo_OidcServer.Models
{
    public class CodeItem
    {
        public AuthenticationRequestModel RequestModel { get; set; }
        public string User { get; set; }
        public string[] Scopes { get; set; }
    }
}
