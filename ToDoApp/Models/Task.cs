using System.ComponentModel.DataAnnotations;
using ToDoApp.Extention;

namespace ToDoApp.Models
{
    public class Task
    {
        public int Id { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string? Title { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        public string? Description { get; set; }

        [Required]
        public StatusEnum Status { get; set; }

        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }
    }
}
