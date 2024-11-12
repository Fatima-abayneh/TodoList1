using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TodoList1.Models;

namespace TodoList1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
