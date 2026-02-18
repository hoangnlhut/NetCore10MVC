namespace MiddlewareDemo.Models
{
    public class ApiKeyModel
    {
        public string ApiKey { get; set; } = string.Empty;
        public UserProfile? User { get; set; }
    }

    public class UserProfile
    {
        public int UserId { get; set; } 
        public string UserName { get; set; } = string.Empty;
    }
}
