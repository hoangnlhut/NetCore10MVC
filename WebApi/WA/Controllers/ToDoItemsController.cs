using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoRepository;

namespace WA.Controllers
{
    [Route("api/todoitems")]
    [ApiController]
    public class ToDoItemsController : ControllerBase
    {
        private readonly IToDoItemRepository _repository;

        public ToDoItemsController(IToDoItemRepository context)
        {
            _repository = context;
        }

        // GET: api/ToDoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoItem?>>> GetToDoItems()
        {
            return Ok(await _repository.GetToDoItemsAsync());
        }

        // GET: api/ToDoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItem?>> GetToDoItem(Guid id)
        {
            var toDoItem = await _repository.GetItemByIdAsync(id);

            return toDoItem == null ? NotFound() : Ok(toDoItem);
        }

        // POST: api/ToDoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostToDoItem(ToDoItem toDoItem)
        {
            return Ok(await _repository.AddToDoItemAsync(toDoItem));
        }

        // PUT: api/ToDoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDoItem(Guid id, ToDoItem toDoItem)
        {
            if (id != toDoItem.Id)
            {
                return BadRequest();
            }

            if (! await _repository.ToDoItemExists(id))
            {
                return NotFound();
            }

            try
            {
                await _repository.UpdateToDoItem(id, toDoItem);
                return Ok("Update successfully");
            }
            catch (DbUpdateConcurrencyException exDb)
            {
                var errorResponse = new { message = $"An internal error occurred while processing the request. Error: {exDb.Message}" };
                // Log the error details here if needed
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = new { message = $"An internal error occurred while processing the request. Error: {ex.Message}" };
                // Log the error details here if needed
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        

        // DELETE: api/ToDoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoItem(Guid id)
        {
            var toDoItem = await _repository.GetItemByIdAsync(id);

            if (toDoItem == null)
            {
                return NotFound();
            }

            await _repository.DeleteToDoItem(toDoItem);
            return Ok("Deleted Successfully");
        }
    }
}
