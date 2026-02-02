using Microsoft.AspNetCore.Mvc.Rendering;

namespace ToDoApp.Models
{
    public class TaskViewModel
    {
        public List<Task>? Tasks { get; set; }
        public SelectList? Statuses { get; set; }
        public string? TaskStatus { get; set; }
        public string? SearchString { get; set; }
    }
}
