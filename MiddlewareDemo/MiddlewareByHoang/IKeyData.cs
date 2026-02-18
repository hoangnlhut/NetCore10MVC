using MiddlewareDemo.Models;

namespace MiddlewareDemo.MiddlewareByHoang
{
    public interface IKeyData
    {
        public IEnumerable<ApiKeyModel> GetApiKeyData();
    }
}
