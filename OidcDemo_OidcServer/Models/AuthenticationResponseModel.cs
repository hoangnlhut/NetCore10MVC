using System.Text.Json.Serialization;

namespace OidcDemo_OidcServer.Models
{
    public class AuthenticationResponseModel : RefreshResponseModel
    {
        [JsonPropertyName("id_token")]
        public required string IdToken { get; set; }
    }
}