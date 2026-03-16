using OidcServer.Models;

namespace OidcServer.Repository
{
    public class CodeItemRepository : ICodeItemRepository
    {
        private readonly Dictionary<string, CodeItem> _listCodeItem = [];
        public void Add(string code, CodeItem item)
        {
            _listCodeItem.Add(code, item);
        }

        public void DeleteByCode(string code)
        {
            _listCodeItem.Remove(code);
        }

        public CodeItem? FindByCode(string code)
        {
           return _listCodeItem.TryGetValue(code, out var item) ? item : null;
        }
    }
}
