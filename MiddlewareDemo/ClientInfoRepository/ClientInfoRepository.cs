using MiddlewareDemo.Models;

namespace MiddlewareDemo.ClientInfoRepository
{
    public class ClientInfoRepository : IClientInfoRepository
    {
        public ClientInfo? GetClientInfo(string apiKey)
        {
            if (apiKey == null) throw new ArgumentNullException(nameof(apiKey));

            return apiKey == "ABCDEF" ? _clientInfos["123"] : null;
        }

        private Dictionary<string, ClientInfo> _clientInfos = new Dictionary<string, ClientInfo>() {
            { "123", new ClientInfo(){ Id = 1 , Name = "Client 1"} },
            { "456", new ClientInfo(){ Id = 1 , Name = "Client 2"} },
            { "789", new ClientInfo(){ Id = 1 , Name = "Client 3"} },
        };
    }
}
