using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Ghtk.Api.Models
{
    public class ActionResultCommon
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; } = default!;
        [JsonPropertyName("error")]
        public OrderCreationResponseError? Error { get; set; }
    }



    public class OrderCreationResponseError
    {
        [JsonPropertyName("code")] public string Code { get; set; } = default!;
        [JsonPropertyName("partner_id")] public string PartnerId { get; set; } = default!;
        [JsonPropertyName("ghtk_label")] public string GhtkLabel { get; set; } = default!;
        [JsonPropertyName("created")] public string Created { get; set; } = default!;
        [JsonPropertyName("status")] public int Status { get; set; }
    }
}
