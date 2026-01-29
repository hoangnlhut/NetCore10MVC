using System.ComponentModel.DataAnnotations;
using ToDoApp.Extention;

namespace ToDoApp.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public string? Description { get; set; }
        public StatusEnum Status { get; set; }
    }
}
