namespace GhtkRepository
{
    public interface IOrderRepository
    {
        Task CreateOrderAsync(Order order);
        Task<Order> GetOrderAsync(string trackingId, string partnerId);
        Task<bool> CancelOrderAsync(string trackingId, string partnerId);
    }
}
