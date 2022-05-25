namespace RavenTodoApp.Persistence;

public class RavenDbRepository<T> : IRepository<T>
{
    private readonly IRavenDbContext _ravenContext;
    
    public RavenDbRepository(IRavenDbContext ravenContext)
    {
        _ravenContext = ravenContext;
    }
    
    public T Get(string id)
    {
        using var session = _ravenContext.Store.OpenSession();
        var element = session.Load<T>(id);
        return element;
    }

    public IEnumerable<T> GetAll(int pageSize, int pageNumber)
    {
        using var session = _ravenContext.Store.OpenSession();
        var elements = session.Query<T>()
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize);
        return elements;
    }

    public void InsertOrUpdate(T element)
    {
        using var session = _ravenContext.Store.OpenSession();
        session.Store(element);
        session.SaveChanges();
    }
}