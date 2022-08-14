using System.ComponentModel.DataAnnotations;

namespace RavenTodoApp;

public class User
{
    [Key]
    public string? Id { get; set; }
    
    public string? UserToken { get; set; }
}