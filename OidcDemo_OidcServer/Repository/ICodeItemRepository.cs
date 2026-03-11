using OidcDemo_OidcServer.Models;

namespace OidcDemo_OidcServer.Repository
{
    public interface ICodeItemRepository
    {
        CodeItem? FindByCode(string code);
        void Add(string code, CodeItem item);
        void Delete(string code);
    }
}
