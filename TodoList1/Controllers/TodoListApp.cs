using Microsoft.AspNetCore.Mvc;

namespace TodoList1.Controllers
{
    public class TodoListApp : Controller
    {
        public IActionResult TodoIndex()
        {
            return View();
        }
    }
}
