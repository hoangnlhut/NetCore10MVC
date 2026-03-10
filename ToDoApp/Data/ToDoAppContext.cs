using Microsoft.EntityFrameworkCore;

namespace ToDoApp.Data
{
    public class ToDoAppContext : DbContext
    {
        public ToDoAppContext(DbContextOptions<ToDoAppContext> options)
            : base(options)
        {
        }

        public DbSet<ToDoApp.Models.Task> Task { get; set; } = default!;
    }
}
