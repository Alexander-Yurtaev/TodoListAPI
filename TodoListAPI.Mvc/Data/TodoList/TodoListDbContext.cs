using Microsoft.EntityFrameworkCore;

namespace TodoListAPI.Mvc.Data.TodoList;

public class TodoListDbContext : DbContext
{
    public DbSet<TodoItem> TodoItems { get; set; }

    public TodoListDbContext(DbContextOptions<TodoListDbContext> options) : base(options)
    {
        
    }
}