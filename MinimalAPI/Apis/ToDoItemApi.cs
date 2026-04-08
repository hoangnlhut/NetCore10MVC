using Microsoft.EntityFrameworkCore;
using MinimalAPI.Data;
using MinimalAPI.Infrastructure.Repositories;
using MinimalAPI.Models;

namespace MinimalAPI.Apis
{
    public static class ToDoItemApi
    {
        public static IEndpointRouteBuilder MapToDoItemApi(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("/todoitems", async (IToDoItemRepository db) => await db.GetAllAsync());

            builder.MapGet("/todoitems/complete", async (IToDoItemRepository db) =>
                await db.GetCompletedAsync());

            builder.MapGet("/todoitems/{id}", async (int id, IToDoItemRepository db) =>
               await db.GetByIdAsync(id)
                   is Todo todo
                       ? Results.Ok(todo)
                       : Results.NotFound());


            builder.MapPost("/todoitems", async (Todo todo, IToDoItemRepository db) =>
            {
                await db.AddAsync(todo);

                return Results.Created($"/todoitems/{todo.Id}", todo);
            });

            builder.MapPut("/todoitems/{id}", async (int id, Todo inputTodo, IToDoItemRepository db) =>
            {
                return await db.UpdateAsync(id, inputTodo)
                    ? Results.NoContent()
                    : Results.NotFound();
            });

            builder.MapDelete("/todoitems/{id}", async (int id, IToDoItemRepository db) =>
            {

                return await db.DeleteAsync(id)
                    ? Results.NoContent()
                    : Results.NotFound();
            });

            builder.MapGet("/todoitems/{id}/title", GetToDoTitle);

            return builder;
        }

        private static async Task<IResult> GetToDoTitle(int id, IToDoItemRepository db)
        {
            var title = await db.GetTitleByIdAsync(id);
            return !string.IsNullOrEmpty(title) ? Results.Ok(title) : Results.NotFound();
        }
    }
}
