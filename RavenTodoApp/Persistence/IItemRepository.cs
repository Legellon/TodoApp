namespace RavenTodoApp.Persistence;

public interface IItemRepository : IRepository<Item>
{
    IEnumerable<Item> GetAllRelated(string userId);
}