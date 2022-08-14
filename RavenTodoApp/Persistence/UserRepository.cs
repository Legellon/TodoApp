namespace RavenTodoApp.Persistence;

public interface IUserRepository : IRepository<User>
{
    User? GetUserByToken(string? userToken);
}

public class UserRepository : RavenDbRepository<User>, IUserRepository
{
    private readonly IRavenDbContext _context;
    
    public UserRepository(IRavenDbContext context) : base(context)
    {
        _context = context;
    }

    public User? GetUserByToken(string? userToken)
    {
        using var session = _context.Store.OpenSession();

        var user = session
            .Query<User>()
            .FirstOrDefault(u => u.UserToken == userToken);

        return user;
    }
}