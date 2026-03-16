using OidcServer.Models;

namespace OidcServer.Repository
{
    public interface ICodeItemRepository
    {
        void Add(string code, CodeItem item);
        CodeItem? FindByCode(string code);
        void DeleteByCode(string code);
    }
}
