using MiddlewareDemo.Models;

namespace MiddlewareDemo.MiddlewareByHoang
{
    public class MemoryKeyData : IKeyData
    {
        public IEnumerable<ApiKeyModel> GetApiKeyData()
        {
            return new List<ApiKeyModel>
            {
                new ApiKeyModel { ApiKey = "7783E70A-B481-4FCD-8384-55D697701CD1", User = new(){  UserId = 1, UserName = "hoangnl1" } },
                new ApiKeyModel { ApiKey = "7783E70A-B481-4FCD-8384-55D697701CD2", User = new(){  UserId = 2, UserName = "hoangnl2" } },
                new ApiKeyModel { ApiKey = "7783E70A-B481-4FCD-8384-55D697701CD3", User = new(){  UserId = 3, UserName = "hoangnl3" } },
                new ApiKeyModel { ApiKey = "7783E70A-B481-4FCD-8384-55D697701CD4", User = new(){  UserId = 4, UserName = "hoangnl4" } },
                new ApiKeyModel { ApiKey = "7783E70A-B481-4FCD-8384-55D697701CD5", User = new(){  UserId = 5, UserName = "hoangnl5" } },
                new ApiKeyModel { ApiKey = "7783E70A-B481-4FCD-8384-55D697701CD6", User = new(){  UserId = 6, UserName = "hoangnl6" } },
                new ApiKeyModel { ApiKey = "7783E70A-B481-4FCD-8384-55D697701CD7", User = new(){  UserId = 7, UserName = "hoangnl7" } },
                new ApiKeyModel { ApiKey = "7783E70A-B481-4FCD-8384-55D697701CD8", User = new(){  UserId = 8, UserName = "hoangnl8" } },
                new ApiKeyModel { ApiKey = "7783E70A-B481-4FCD-8384-55D697701CD9", User = new(){  UserId = 9, UserName = "hoangnl9" } },
                new ApiKeyModel { ApiKey = "7783E70A-B481-4FCD-8384-55D697701C10", User = new(){  UserId = 10, UserName = "hoangnl10" } },
            };
        }
    }
}
