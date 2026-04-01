using Microsoft.EntityFrameworkCore;

namespace ToDoRepository
{
    public class SqlServerToDoItemRepository : IToDoItemRepository
    {
        private readonly ToDoContext _context;

        public SqlServerToDoItemRepository(ToDoContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<ToDoItem>> GetToDoItemsAsync()
        {
            return await _context.ToDoItems.ToListAsync();
        }


        public async Task<ToDoItem?> GetItemByIdAsync(Guid id)
        {
            return await _context.ToDoItems.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ToDoItem> AddToDoItemAsync(ToDoItem item)
        {
            var entity = _context.ToDoItems.Add(item);
            await _context.SaveChangesAsync();

            return entity.Entity;
        }

        public async Task UpdateToDoItem(Guid id, ToDoItem toDoItem)
        {
            _context.Entry(toDoItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteToDoItem(ToDoItem toDoItem)
        {
            _context.ToDoItems.Remove(toDoItem);
            await _context.SaveChangesAsync();
        }


        public async Task<bool>ToDoItemExists(Guid id)
        {
            return await _context.ToDoItems.AnyAsync(e => e.Id == id);
        }
    }
}
