using System.Text.Json;

namespace SessionFeature.MySession
{
    public class FileMySessionStorageEngine : IMySessionStorageEngine
    {
        private readonly string _directoryPath;

        public FileMySessionStorageEngine(string directoryPath)
        {
            _directoryPath = directoryPath;
        }

        public async Task CommitAsync(string id, Dictionary<string, byte[]> store, CancellationToken cancellationToken)
        {
            var filePath = Path.Combine(_directoryPath, id);
            var json = JsonSerializer.Serialize(store);
            await File.WriteAllTextAsync(filePath, json, cancellationToken);
        }

        public async Task<Dictionary<string, byte[]>> LoadAsync(string id, CancellationToken cancellationToken)
        {
            var filePath = Path.Combine(_directoryPath, id);

            if (!File.Exists(filePath))
            {
                return new Dictionary<string, byte[]>();
            }

            var json = await File.ReadAllTextAsync(filePath, cancellationToken);
            return JsonSerializer.Deserialize<Dictionary<string, byte[]>>(json) ?? new Dictionary<string, byte[]>();
        }
    }
}
