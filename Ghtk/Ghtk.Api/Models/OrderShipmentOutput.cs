using Newtonsoft.Json;

namespace Ghtk.Api.Models
{
    public class OrderShipmentOutput : ActionResultCommon
    {
        [JsonProperty("order")]
        public OrderOutput Order { get; set; }
    }

    public class OrderOutput
    {
        [JsonProperty("partner_id")] public string PartnerId { get; set; }
        [JsonProperty("label")] public string Label { get; set; }
        [JsonProperty("area")] public string Area { get; set; }
        [JsonProperty("fee")] public string Fee { get; set; }
        [JsonProperty("insurance_fee")] public string InsuranceFee { get; set; }
        [JsonProperty("tracking_id")] public int TrackingId { get; set; }
        [JsonProperty("estimated_pick_time")] public string EstimatedPickTime { get; set; }
        [JsonProperty("estimated_deliver_time")] public string EstimatedDeliverTime { get; set; }
        [JsonProperty("products")] public int Products { get; set; }
        [JsonProperty("status_id")] public int StatusId { get; set; }
    }

    public class OrderShipmentErrorOutput
    {
        [JsonProperty("code")] public string Code { get; set; }
        [JsonProperty("partner_id")] public string PartnerId { get; set; }
        [JsonProperty("ghtk_label")] public string GhtkLabel { get; set; }
        [JsonProperty("created")] public string Created { get; set; }
        [JsonProperty("status")] public int Status { get; set; }
    }
}
