using TodoListAPI.Mvc.Data.TodoList;

namespace TodoListAPI.Mvc.Repositories;

public interface ITodoRepository
{
    Task<TodoItem> Create(TodoItem item);
    Task Update(int id, TodoItem item);
    Task Delete(int id);
    Task<IEnumerable<TodoItem>> GetAll(int? page=0, int? pageSize=10);
    Task<TodoItem> Get(int id);
}
