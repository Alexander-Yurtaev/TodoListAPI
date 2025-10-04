using Microsoft.EntityFrameworkCore;

namespace TodoListAPI.Mvc.Data.TodoList;

public class TodoListDbContext : DbContext
{
    public TodoListDbContext(DbContextOptions<TodoListDbContext> options) : base(options)
    {
        
    }
}