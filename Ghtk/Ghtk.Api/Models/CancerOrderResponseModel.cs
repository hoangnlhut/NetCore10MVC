using System.Text.Json.Serialization;

namespace Ghtk.Api.Models
{
    public class CancerOrderResponseModel : ActionResultCommon
    {
        [JsonPropertyName("log_id")]
        public string LogId { get; set; }
    }
}
