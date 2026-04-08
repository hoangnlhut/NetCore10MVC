using MinimalAPI.Models;

namespace MinimalAPI.Infrastructure.Repositories
{
    public interface IToDoItemRepository
    {
            Task<IEnumerable<Todo>> GetAllAsync();
            Task<Todo?> GetByIdAsync(int id);
            Task<Todo?> GetCompletedAsync();
            Task<string?> GetTitleByIdAsync(int id);
            Task AddAsync(Todo todo);
            Task<bool> UpdateAsync(int id, Todo todo);
            Task<bool> DeleteAsync(int id);
    }
}
