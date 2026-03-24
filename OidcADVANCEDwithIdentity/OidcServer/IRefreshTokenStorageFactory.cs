namespace OidcServer
{
    public interface IRefreshTokenStorageFactory
    {
        IRefreshTokenStorage GetTokenStorage();
        IRefreshTokenStorage GetInvalidatedTokenStorage();
    }
}