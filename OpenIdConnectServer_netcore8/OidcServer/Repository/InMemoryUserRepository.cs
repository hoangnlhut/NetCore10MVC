using OidcServer.Models;

namespace OidcServer.Repository
{
    public class InMemoryUserRepository : IUserRepository
    {
        private List<User> _users = new List<User>
        {
            new User() { Name = "alice" },
            new User() { Name = "bob" },
            new User() { Name = "hoang" },
        };

        public void DeleteByName(string name)
        {
           var user =  FindByName(name);

            if (user is null) {
                throw new ArgumentException("User not found");
            }

            _users.Remove(user);
        }

        public User? FindByName(string name)
        {
            return _users.FirstOrDefault(u => u.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
