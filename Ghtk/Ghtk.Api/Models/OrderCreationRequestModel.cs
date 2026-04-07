namespace Ghtk.Api.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class OrderCreationRequestModel
    {
        [JsonPropertyName("products")]
        public List<Product> Products { get; set; }

        [JsonPropertyName("order")]
        public OrderCreationRequestOrder Order { get; set; }
    }

    public class OrderCreationRequestOrder
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("pick_name")]
        public string PickName { get; set; } = default!;

        [JsonPropertyName("pick_address")]
        public string PickAddress { get; set; } = default!;    

        [JsonPropertyName("pick_province")]
        public string PickProvince { get; set; } = default!;

        [JsonPropertyName("pick_district")]
        public string PickDistrict { get; set; } = default!;

        [JsonPropertyName("pick_ward")]
        public string PickWard { get; set; } = default!;

        [JsonPropertyName("pick_tel")]
        public string PickTel { get; set; } = default!;

        [JsonPropertyName("tel")]
        public string Tel { get; set; } = default!;

        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("address")]
        public string Address { get; set; } = default!;

        [JsonPropertyName("province")]
        public string Province { get; set; } = default!;

        [JsonPropertyName("district")]
        public string District { get; set; } = default!;

        [JsonPropertyName("ward")]
        public string Ward { get; set; } = default!;

        [JsonPropertyName("hamlet")]
        public string Hamlet { get; set; } = default!;

        [JsonPropertyName("is_freeship")]
        public int IsFreeship { get; set; }

        [JsonPropertyName("pick_date")]
        public string PickDate { get; set; }

        [JsonPropertyName("pick_money")]
        public long PickMoney { get; set; }

        [JsonPropertyName("note")]
        public string Note { get; set; } = default!;

        [JsonPropertyName("value")]
        public long Value { get; set; }

        [JsonPropertyName("transport")]
        public string Transport { get; set; } = default!;

        [JsonPropertyName("pick_option")]
        public string PickOption { get; set; } = default!;

        [JsonPropertyName("gam_solutions")]
        public List<GamSolution> GamSolutions { get; set; } = [];
    }

    public class GamSolution
    {
        [JsonPropertyName("solution_id")]
        public long SolutionId { get; set; }
    }

    public class Product
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }  = default!;

        [JsonPropertyName("weight")]
        public double Weight { get; set; }

        [JsonPropertyName("quantity")]
        public long Quantity { get; set; }

        [JsonPropertyName("product_code")]
        public long ProductCode { get; set; }
    }
}
