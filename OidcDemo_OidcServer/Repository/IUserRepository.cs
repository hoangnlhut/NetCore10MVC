using OidcDemo_OidcServer.Models;

namespace OidcDemo_OidcServer.Repository
{
    public interface IUserRepository
    {
        User? FindByUsername(string username);
    }
}
