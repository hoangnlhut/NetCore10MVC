namespace ToDoRepository
{
    public interface IToDoItemRepository
    {
        Task<ToDoItem?> GetItemByIdAsync(Guid id);
        Task<IEnumerable<ToDoItem>> GetToDoItemsAsync();
        Task<ToDoItem> AddToDoItemAsync(ToDoItem item);
       
        Task UpdateToDoItem(Guid id, ToDoItem item);
        Task DeleteToDoItem(ToDoItem item);
        Task<bool> ToDoItemExists(Guid id);
    }
}
