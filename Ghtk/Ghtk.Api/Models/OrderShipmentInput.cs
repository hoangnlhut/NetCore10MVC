using Newtonsoft.Json;

namespace Ghtk.Api.Models
{
    public class OrderShipmentInput
    {
        [JsonProperty("order")] public Order Order{ get; set; }
        [JsonProperty("products")]  public IEnumerable<Product> Products { get; set; }
    }

    public class Order
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("pick_name")] public string? PickName { get; set; }
        [JsonProperty("pick_money")] public int PickMoney { get; set; }
        [JsonProperty("pick_address")] public string? PickAddress { get; set; }
        [JsonProperty("pick_province")] public string? PickProvince { get; set; }
        [JsonProperty("pick_district")] public string? PickDistrict { get; set; }
        [JsonProperty("pick_ward")] public string? PickWard { get; set; }
        [JsonProperty("pick_tel")] public string? PickTel { get; set; }
        [JsonProperty("tel")] public string? Tel { get; set; }
        [JsonProperty("name")] public string? Name { get; set; }
        [JsonProperty("address")] public string? Address { get; set; }
        [JsonProperty("province")] public string? Province { get; set; }
        [JsonProperty("district")] public string? District { get; set; }
        [JsonProperty("ward")] public string? Ward { get; set; }
        [JsonProperty("hamlet")] public string? Hamlet { get; set; }
        [JsonProperty("is_freeship")] public string? IsFreeship { get; set; }
        [JsonProperty("pick_date")] public string? PickDate { get; set; }
        [JsonProperty("note")] public string? Note { get; set; }
        [JsonProperty("value")] public int Value { get; set; }
        [JsonProperty("transport")] public string? Transport { get; set; }
        [JsonProperty("pick_option")] public string? PickOption { get; set; }
        [JsonProperty("gam_solutions")] public IEnumerable<GamSolution>? GamSolutions { get; set; }

    }

    public class GamSolution
    {
        [JsonProperty("solution_id")] public int SolutionId { get; set; }
    }

    public class Product
    {
        [JsonProperty("name")] public string? Name { get; set; }
        [JsonProperty("weight")] public float Weight { get; set; }
        [JsonProperty("quantity")] public int Quantity { get; set; }
        [JsonProperty("product_code")] public int ProductCode { get; set; }
    }
}