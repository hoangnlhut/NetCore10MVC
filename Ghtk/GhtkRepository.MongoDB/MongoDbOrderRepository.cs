using MongoDB.Driver;

namespace GhtkRepository
{
    public class MongoDbOrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<Order> _ordersCollection;
        private readonly MongoClient _client;
        public MongoDbOrderRepository(MongoClient mongoClient)
        {
            _client = mongoClient;
            var mongoDatabase = _client.GetDatabase("Ghtk");
            _ordersCollection = mongoDatabase.GetCollection<Order>("Order");
        }
        public Task CreateOrderAsync(Order order)
        {
            return _ordersCollection.InsertOneAsync(order);
        }

        public async Task<bool> CancelOrderAsync(string trackingId, string partnerId)
        {
            var filter = Builders<Order>.Filter.Eq(x => x.TrackingId, trackingId) & Builders<Order>.Filter.Eq(x => x.PartnerId, partnerId);
            var update = Builders<Order>.Update.Set(x => x.Status, 9).Set(x => x.Modified, DateTime.UtcNow);

            var row = await _ordersCollection.UpdateOneAsync(filter, update);
            return row.ModifiedCount > 0;
        }

        public Task<Order> GetOrderAsync(string trackingId, string partnerId)
        {
           return _ordersCollection.Find(x => x.TrackingId == trackingId && x.PartnerId == partnerId).FirstOrDefaultAsync();
        }
        
    }
}
