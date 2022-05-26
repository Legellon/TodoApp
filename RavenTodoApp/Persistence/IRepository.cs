namespace RavenTodoApp.Persistence;

public interface IRepository<T>
{
    public T Get(string id);
    public void Delete(string id);
    public IEnumerable<T> GetAll(int pageSize, int pageNumber);
    public void InsertOrUpdate(T element);
}