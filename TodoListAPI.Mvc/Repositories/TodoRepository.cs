using Microsoft.EntityFrameworkCore;
using TodoListAPI.Mvc.Data.TodoList;
using TodoListAPI.Mvc.Repositories.Exceptions;

namespace TodoListAPI.Mvc.Repositories;

public class TodoRepository : ITodoRepository
{
    private readonly TodoListDbContext _db;

    public TodoRepository(TodoListDbContext db)
    {
        _db = db;
    }

    public async Task<TodoItem> Create(TodoItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentException.ThrowIfNullOrEmpty(item.Title);
        ArgumentException.ThrowIfNullOrEmpty(item.Description);

        if (item.OwnerId <= 0)
        {
            throw new ArgumentException("OwnerId must be initialized.");
        }

        await _db.AddAsync(item);
        await _db.SaveChangesAsync();
        return item;
    }

    public async Task Delete(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Id must be greater that 0.");
        }

        var item = await _db.TodoItems.FindAsync(id);
        if (item is null)
        {
            throw new NotFoundException($"TodoItem with id={id} not found.");
        }

        _db.Remove(item);
        await _db.SaveChangesAsync();
    }

    public async Task<TodoItem> Get(int id)
    {
        var item = await _db.TodoItems.FindAsync(id);

        if (item is null)
        {
            throw new NotFoundException($"TodoItem with id={id} not found.");
        }

        return item;
    }

    public async Task<IEnumerable<TodoItem>> GetAll(int? page = 1, int? pageSize = 10)
    {
        // Получение данных с пагинацией
        var items = await _db.TodoItems
            .Skip((page!.Value - 1) * pageSize!.Value)
            .Take(pageSize.Value)
            .ToListAsync();

        return items;
    }

    public async Task Update(int id, TodoItem source)
    {
        var target = await _db.TodoItems.FindAsync(id);
        if (target is null)
        {
            throw new NotFoundException($"TodoItem with id={id} not found.");
        }

        target.Title = source.Title;
        target.Description = source.Description;

        await _db.SaveChangesAsync();
    }
}
