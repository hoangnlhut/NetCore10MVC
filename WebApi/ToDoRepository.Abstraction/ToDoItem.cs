using System.ComponentModel.DataAnnotations;

namespace ToDoRepository
{
    public class ToDoItem
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsComplete { get; set; }

    }
}
