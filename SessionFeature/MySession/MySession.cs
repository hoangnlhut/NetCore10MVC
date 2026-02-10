using System.Diagnostics.CodeAnalysis;

namespace SessionFeature.MySession
{
    public class MySession(string id, IMySessionStorageEngine engine) : ISession
    {
        private readonly Dictionary<string, byte[]> _store = new Dictionary<string, byte[]>();
        public bool IsAvailable { 
            get { 
                Load();
                return true;
            } 
        }

        public string Id => id;

        public IEnumerable<string> Keys => _store.Keys;

        public void Clear()
        {
            _store.Clear();
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
           await engine.CommitAsync(Id, _store, cancellationToken);
        }

        public async Task LoadAsync(CancellationToken cancellationToken = default)
        {
           Clear();
           var loadedStore =  await engine.LoadAsync(Id, cancellationToken);

            foreach (var kvp in loadedStore)
            {
                _store[kvp.Key] = kvp.Value;
            }
        }

        public void Load()
        {
            Clear();
            var loadedStore = engine.Load(Id);

            foreach (var kvp in loadedStore)
            {
                _store[kvp.Key] = kvp.Value;
            }
        }

        public void Remove(string key)
        {
            _store.Remove(key);
        }

        public void Set(string key, byte[] value)
        {
            _store[key] = value;
        }

        public bool TryGetValue(string key, [NotNullWhen(true)] out byte[]? value)
        {
           return _store.TryGetValue(key, out value);
        }
    }
}
