using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Ghtk.Api.Models
{
    public class OrderCreationResponseModel : ActionResultCommon
    {
        [JsonPropertyName("order")]
        public OrderCreationResponseOrder Order { get; set; }
    }

    public class OrderCreationResponseOrder
    {
        [JsonPropertyName("partner_id")]
        public string PartnerId { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; } = default!;

        [JsonPropertyName("area")]
        public string Area { get; set; } = default!;

        [JsonPropertyName("fee")]
        public string? Fee { get; set; }

        [JsonPropertyName("insurance_fee")]
        public string? InsuranceFee { get; set; }

        [JsonPropertyName("tracking_id")]
        public long TrackingId { get; set; } = default!;

        [JsonPropertyName("estimated_pick_time")]
        public string EstimatedPickTime { get; set; } = default!;

        [JsonPropertyName("estimated_deliver_time")]
        public string EstimatedDeliverTime { get; set; } = default!;

        [JsonPropertyName("products")]
        public List<Product> Products { get; set; }

        [JsonPropertyName("status_id")]
        public long StatusId { get; set; }
    } 
}
