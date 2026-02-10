namespace SessionFeature.MySession
{
    public interface IMySessionStorage
    {
        public ISession Create();
        public ISession Get(string id);
    }
}
