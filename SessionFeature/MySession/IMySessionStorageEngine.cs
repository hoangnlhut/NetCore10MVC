namespace SessionFeature.MySession
{
    public interface IMySessionStorageEngine
    {
        Task<Dictionary<string, byte[]>> LoadAsync(string id, CancellationToken cancellationToken);
        Dictionary<string, byte[]> Load(string id);
        Task CommitAsync(string id, Dictionary<string, byte[]> store, CancellationToken cancellationToken);

    }
}
