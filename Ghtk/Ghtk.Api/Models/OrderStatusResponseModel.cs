using System.Text.Json.Serialization;

namespace Ghtk.Api.Models
{
    public class OrderStatusResponseModel : ActionResultCommon
    {
        [JsonPropertyName("order")]
        public OrderStatusResponseOrder Order { get; set; }
    }

    public class OrderStatusResponseOrder
    {
        [JsonPropertyName("label_id")]
        public string LabelId { get; set; }

        [JsonPropertyName("partner_id")]
        public string PartnerId { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("status_text")]
        public string StatusText { get; set; }

        [JsonPropertyName("created")]
        public string Created { get; set; }

        [JsonPropertyName("modified")]
        public string Modified { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("pick_date")]
        public string PickDate { get; set; }

        [JsonPropertyName("deliver_date")]
        public string DeliverDate { get; set; }

        [JsonPropertyName("customer_fullname")]
        public string CustomerFullname { get; set; }

        [JsonPropertyName("customer_tel")]
        public string CustomerTel { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("storage_day")]
        public string StorageDay { get; set; }

        [JsonPropertyName("ship_money")]
        public string ShipMoney { get; set; }

        [JsonPropertyName("insurance")]
        public string Insurance { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("weight")]
        public string Weight { get; set; }

        [JsonPropertyName("pick_money")]
        public long PickMoney { get; set; }

        [JsonPropertyName("is_freeship")]
        public string IsFreeship { get; set; }
    }
}
