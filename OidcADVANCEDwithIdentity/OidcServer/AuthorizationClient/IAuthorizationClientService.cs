namespace OidcServer.AuthorizationClient
{
    public interface IAuthorizationClientService
    {
        AuthorizationClient? FindById(string id);
    }
}
