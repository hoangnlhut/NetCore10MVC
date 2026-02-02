using Microsoft.AspNetCore.Mvc.Rendering;

namespace ToDoApp.Models
{
    public class TaskViewModel : TaskCreateViewModel
    {
        public List<Task>? Tasks { get; set; }
        public string? TaskStatus { get; set; }
        public string? SearchString { get; set; }
    }

    public class TaskCreateViewModel
    {
        public SelectList? Statuses { get; set; }
        public Models.Task? Task { get; set; }

    }
}
