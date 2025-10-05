using System.ComponentModel.DataAnnotations;

namespace TodoListAPI.Mvc.Data.TodoList;

public class TodoItem
{
    public TodoItem(string title, string description, int ownerId)
    {
        Title = title;
        Description = description;
        OwnerId = ownerId;
    }

    public int Id { get; set; }

    [MaxLength(125)]
    public string Title { get; set; }

    [MaxLength(255)]
    public string Description { get; set; }

    public int OwnerId { get; set; }
}
