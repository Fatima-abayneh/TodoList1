using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TodoList1.Data;
using TodoList1.Models;

namespace TodoList1.Controllers
{
    public class TodoController : Controller
    {
        private readonly Data.ApplicationDbContext _context;

        public TodoController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(string searchString)
        {
            if (_context.TodoItems == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TodoItems'  is null.");
            }

            var todoitem = from m in _context.TodoItems
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                todoitem = todoitem.Where(s => s.Title!.ToUpper().Contains(searchString.ToUpper()));
            }

            return View(todoitem.ToList());
        }
       
        [HttpPost]
        public ActionResult AddTodoItem(TodoItem item)
        {
            if (ModelState.IsValid)
            {
                _context.TodoItems.Add(item);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TodoItems == null)
            {
                return NotFound();
            }

            var todoitem = await _context.TodoItems.FindAsync(id);
            if (todoitem == null)
            {
                return NotFound();
            }
            return View(todoitem);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description, DueDate, IsCompleted")] TodoItem todoitem)
        {
            if (id != todoitem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(todoitem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoitemExists(todoitem.Id))
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
            return View(todoitem);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TodoItems == null)
            {
                return NotFound();
            }

            var todoitem = await _context.TodoItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todoitem == null)
            {
                return NotFound();
            }

            return View(todoitem);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TodoItems == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Categories'  is null.");
            }
            var category = await _context.TodoItems.FindAsync(id);
            if (category != null)
            {
                _context.TodoItems.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public ActionResult ToggleCompletion(int id)
        {
            var item = _context.TodoItems.Find(id);
            if (item != null)
            {
                item.IsCompleted = !item.IsCompleted;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult UpdateStatus(int id, bool isCompleted)
        {
            var item = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if (item != null)
            {
                item.IsCompleted = isCompleted;  // Update the status
                _context.Update(item);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        private bool TodoitemExists(int id)
        {
            return (_context.TodoItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
