using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApp.Data;
using ToDoApp.Extention;
using ToDoApp.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ToDoApp.Controllers
{
    public class TasksController : Controller
    {
        private readonly ToDoAppContext _context;

        public TasksController(ToDoAppContext context)
        {
            _context = context;
        }

        // GET: Tasks
        public async Task<IActionResult> Index(string taskStatus, string searchString)
        {
            if (_context.Task == null)
            {
                return Problem("Entity set 'MvcToDoContext.Task'  is null.");
            }

            // Use LINQ to get list of genres.
            IQueryable<StatusEnum> statusQuery = from m in _context.Task
                                            orderby m.Status
                                            select m.Status;
            var tasks = from m in _context.Task
                         select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                tasks = tasks.Where(s => s.Title!.ToUpper().Contains(searchString.ToUpper()));
            }

            if (!string.IsNullOrEmpty(taskStatus) && Enum.TryParse(taskStatus, out StatusEnum myStatus))
            {
                tasks = tasks.Where(x => x.Status == myStatus);
            }

            var taskVM = new TaskViewModel
            {
                Statuses = new SelectList(await statusQuery.Distinct().ToListAsync()),
                Tasks = await tasks.ToListAsync()
            };

            return View(taskVM);
        }

        // GET: Tasks/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var task = await _context.Task
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (task == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(task);
        //}

        // GET: Tasks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Description,Status")] ToDoApp.Models.Task task)
        {
            if (ModelState.IsValid)
            {
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Task.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Description,Status")] ToDoApp.Models.Task task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Task
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.Task.FindAsync(id);
            if (task != null)
            {
                _context.Task.Remove(task);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ChangeStatus(int id, int status)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var task = await _context.Task.FindAsync(id);
                    if(task is null) return NotFound();

                    task.Status = (Extention.StatusEnum)status;
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TaskExists(int id)
        {
            return _context.Task.Any(e => e.Id == id);
        }
    }
}
