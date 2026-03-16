using OidcDemo_OidcServer.Models;

namespace OidcDemo_OidcServer.Repository
{
    public class InMemoryCodeItemRepository : ICodeItemRepository
    {
        private readonly Dictionary<string, CodeItem> _list = [];
        public void Add(string code, CodeItem item)
        {
            _list.Add(code, item);
        }

        public void Delete(string code)
        {
            var model = FindByCode(code);
            if (model is null)
            {
                throw new InvalidOperationException($"Code {code} not found");
            }

            _list.Remove(code);
        }

        public CodeItem? FindByCode(string code)
        {
            return _list.TryGetValue(code, out var codeItem) ? codeItem : null;
        }
    }
}
