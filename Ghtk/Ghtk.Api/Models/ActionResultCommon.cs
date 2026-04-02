using Newtonsoft.Json;

namespace Ghtk.Api.Models
{
    public class ActionResultCommon
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("message")]
        public int Message { get; set; }
        [JsonProperty("error")]
        public string? Error { get; set; }
    }
}
