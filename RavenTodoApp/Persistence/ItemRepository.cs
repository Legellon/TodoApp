namespace RavenTodoApp.Persistence;

public class ItemRepository : RavenDbRepository<Item>, IItemRepository
{
    private readonly IRavenDbContext _context;
    
    public ItemRepository(IRavenDbContext context) : base(context)
    {
        _context = context;
    }

    public IEnumerable<Item> GetAllRelated(string userId)
    {
        using var session = _context.Store.OpenSession();

        var items =
            session.Query<Item>()
                .Where(x => x.UserId == userId)
                .ToList();

        return items;
    }
}