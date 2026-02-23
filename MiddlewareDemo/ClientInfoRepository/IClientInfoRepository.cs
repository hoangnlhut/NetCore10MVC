using MiddlewareDemo.Models;

namespace MiddlewareDemo.ClientInfoRepository
{
    public interface IClientInfoRepository
    {
        ClientInfo? GetClientInfo(string apiKey);
    }
}
