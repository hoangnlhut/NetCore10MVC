
using System.Text.Json.Serialization;

namespace GhtkRepository
{
    public class Order
    {
        public string Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public string TrackingId { get; set; }
        public string PartnerId { get; set; }
        public string Message { get; set; }
        public string MyProperty { get; set; }
        public string PickName { get; set; } = default!;

        public string PickAddress { get; set; } = default!;

        public string PickProvince { get; set; } = default!;

        public string PickDistrict { get; set; } = default!;

        public string PickWard { get; set; } = default!;

        public string PickTel { get; set; } = default!;

        public string Tel { get; set; } = default!;

        public string Name { get; set; } = default!;

        public string Address { get; set; } = default!;

        public string Province { get; set; } = default!;

        public string District { get; set; } = default!;

        public string Ward { get; set; } = default!;

        public string Hamlet { get; set; } = default!;

        public int IsFreeship { get; set; }

        public DateTime PickDate { get; set; }
        public DateTime DeliverDate { get; set; }

        public long PickMoney { get; set; }

        public string Note { get; set; } = default!;

        public long Value { get; set; }

        public string Transport { get; set; } = default!;

        public string PickOption { get; set; } = default!;

        public List<GamSolution> GamSolutions { get; set; } = [];
        public List<Product> Products { get; set; } = [];
        public int Status { get; set; }
    }


    public class GamSolution
    {
        public long SolutionId { get; set; }
    }

    public class Product
    {
        public string Name { get; set; } = default!;

        public double Weight { get; set; }

        public long Quantity { get; set; }

        public long ProductCode { get; set; }
    }
}
