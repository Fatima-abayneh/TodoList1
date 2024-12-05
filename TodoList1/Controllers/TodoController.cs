using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index()
        {
            var todoItems = _context.TodoItems.ToList();
            return View(todoItems);
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
    }
}
