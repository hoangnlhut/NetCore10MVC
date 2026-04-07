using GhtkRepository;
using MongoDB.Driver;

namespace Ghtk.Api
{
    public static class MongoDbClientExtensions
    {
        public static void AddMongoDbClient(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoConnectString = configuration.GetSection("MongoDatabase").GetValue<string>("ConnectionString") ?? throw new Exception("missing MongoDatabase");

            var mongoClient = new MongoClient(mongoConnectString);

            services.AddSingleton(mongoClient);
        }
    }
}
