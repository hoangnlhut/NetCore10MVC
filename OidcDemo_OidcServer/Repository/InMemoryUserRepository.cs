using OidcDemo_OidcServer.Models;

namespace OidcDemo_OidcServer.Repository
{
    public class InMemoryUserRepository : IUserRepository
    {
        private List<User> _users = new List<User>()
        {
            new User() { Name = "alice" },
            new User() { Name = "bob" },
            new User() { Name = "hoang" }
        };
        public User? FindByUsername(string username)
        {
           return _users.FirstOrDefault(u => u.Name.Equals(username, StringComparison.OrdinalIgnoreCase));
        }
    }
}
