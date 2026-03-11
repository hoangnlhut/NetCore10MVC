using System.Text.Json.Serialization;

namespace OidcDemo_OidcServer.Models
{
    public class CodeFlowResponseViewModel : CodeFlowResponseModel
    {
        [JsonPropertyName("redirect_uri")]
        public required string RedirectUri { get; set; }
    }
}
