using System.ComponentModel.DataAnnotations;

namespace RavenTodoApp;

public class Item
{
    [Key]
    public string? Id { get; set; }
    
    public string? Title { get; set; }
    
    public string? Owner { get; set; }
}