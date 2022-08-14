namespace RavenTodoApp.Persistence;

public interface IItemRepository : IRepository<Item>
{
    IEnumerable<Item> GetAllRelatedItems(string userToken);
}

public class ItemRepository : RavenDbRepository<Item>, IItemRepository
{
    private readonly IRavenDbContext _context;
    
    public ItemRepository(IRavenDbContext context) : base(context)
    {
        _context = context;
    }

    public IEnumerable<Item> GetAllRelatedItems(string userToken)
    {
        using var session = _context.Store.OpenSession();

        var items =
            session
                .Query<Item>()
                .Where(x => x.Owner == userToken)
                .ToList();

        return items;
    }
}