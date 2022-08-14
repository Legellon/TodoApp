namespace RavenTodoApp.Persistence;

public class RavenDbRepository<T> : IRepository<T> where T : class
{
    private readonly IRavenDbContext _context;

    protected RavenDbRepository(IRavenDbContext context)
    {
        _context = context;
    }
    
    public T Get(string id)
    {
        using var session = _context.Store.OpenSession();
        var element = session.Load<T>(id);
        return element;
    }

    public void Delete(string id)
    {
        using var session = _context.Store.OpenSession();
        session.Delete(id);
        session.SaveChanges();
    }

    public IEnumerable<T> GetAll(int pageSize, int pageNumber)
    {
        using var session = _context.Store.OpenSession();
        
        var elements = session.Query<T>()
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize);
        
        return elements;
    }

    public void InsertOrUpdate(T element)
    {
        using var session = _context.Store.OpenSession();
        session.Store(element);
        session.SaveChanges();
    }
}