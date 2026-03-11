namespace OidcDemo_OidcServer.Models
{
    public class CodeItem
    {
        public AuthenticationRequestModel requestModel { get; set; }
        public string user { get; set; }
        public string[] scopes { get; set; }
    }
}
