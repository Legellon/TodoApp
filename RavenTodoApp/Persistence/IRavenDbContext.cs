using Microsoft.Extensions.Options;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Raven.Client.Exceptions.Database;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace RavenTodoApp.Persistence;

public interface IRavenDbContext
{
    public DocumentStore Store { get; }
}

public class RavenDbContext : IRavenDbContext
{
    private readonly PersistenceSettings _persistenceSettings;
    public DocumentStore Store { get; }
    
    public RavenDbContext(IOptionsMonitor<PersistenceSettings> settings)
    {
        _persistenceSettings = settings.CurrentValue;

        Store = new DocumentStore()
        {
            Database = _persistenceSettings.DatabaseName,
            Urls = _persistenceSettings.Urls
        };

        Store.Initialize();
        EnsureDatabaseIsCreated();
    }

    private void EnsureDatabaseIsCreated()
    {
        try
        {
            Store.Maintenance
                .ForDatabase(_persistenceSettings.DatabaseName)
                .Send(new GetStatisticsOperation());
        }
        catch (DatabaseDoesNotExistException ex)
        {
            Store.Maintenance.Server
                .Send(new CreateDatabaseOperation(
                        new DatabaseRecord(_persistenceSettings.DatabaseName)));
        }
    }
}