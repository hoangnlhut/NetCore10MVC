using Microsoft.EntityFrameworkCore;
using ToDoApp.Models;

namespace ToDoApp.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ToDoAppContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ToDoAppContext>>()))
            {
                // Look for any movies.
                if (context.Task.Any())
                {
                    return;   // DB has been seeded
                }
                context.Task.AddRange(
                    new ToDoApp.Models.Task
                    {
                        Title = "To Do App",
                        ReleaseDate = DateTime.Parse("2026-1-29"),
                        DueDate = DateTime.Parse("2026-1-29"),
                        Description = "Create a to do app using Blazor and EF Core",
                        Status = Extention.StatusEnum.InProgress
                    },
                    new ToDoApp.Models.Task
                    {
                        Title = "Complete fundamental .net course",
                        Description = "Complete the .net fundamental course on Pluralsight",
                        ReleaseDate = DateTime.Parse("2026-1-29"),
                        DueDate = DateTime.Parse("2026-1-29"),
                        Status = Extention.StatusEnum.InProgress
                    },
                    new ToDoApp.Models.Task
                    {
                        Title = "Complete advanced .net course",
                        Description = "Complete the .net advanced course on Pluralsight",
                        ReleaseDate = DateTime.Parse("2026-1-29"),
                        DueDate = DateTime.Parse("2026-1-29"),
                        Status = Extention.StatusEnum.InProgress
                    },
                    new ToDoApp.Models.Task
                    {
                        Title = "Complete microservice in .net course",
                        Description = "Complete the microservice in .net course on Pluralsight",
                        ReleaseDate = DateTime.Parse("2026-1-29"),
                        DueDate = DateTime.Parse("2026-1-29"),
                        Status = Extention.StatusEnum.InProgress
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
