using Microsoft.EntityFrameworkCore;
using MinimalAPI.Data;
using MinimalAPI.Models;

namespace MinimalAPI.Infrastructure.Repositories
{
    public class ToDoItemRepository : IToDoItemRepository
    {
        private readonly ApplicationDbContext _context;
        public ToDoItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Todo>> GetAllAsync()
        {
            return await _context.Todos.ToListAsync();
        }
        public async Task<Todo?> GetByIdAsync(int id)
        {
            return await _context.Todos.FindAsync(id);
        }
        public async Task<Todo?> GetCompletedAsync()
        {
            return await _context.Todos.Where(t => t.IsComplete).FirstOrDefaultAsync();
        }
        public async Task<string?> GetTitleByIdAsync(int id)
        {
            return await _context.Todos.Where(t => t.Id == id).Select(t => t.Name).FirstOrDefaultAsync();
        }
        public async Task AddAsync(Todo todo)
        {
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> UpdateAsync(int id, Todo todo)
        {
            var existingTodo = await GetByIdAsync(id);

            if (existingTodo is null) return false;

            existingTodo.Name = todo.Name;
            existingTodo.IsComplete = todo.IsComplete;

            _context.Todos.Update(existingTodo);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo != null)
            {
                _context.Todos.Remove(todo);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
