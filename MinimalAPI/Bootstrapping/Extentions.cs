using MinimalAPI.Infrastructure.Repositories;

namespace MinimalAPI.Bootstrapping
{
    public static class Extentions
    {
        public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
            {
                services.AddScoped<IToDoItemRepository, ToDoItemRepository>();
                return services;
        }
    }
}
