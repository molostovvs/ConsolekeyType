namespace ConsolekeyType.Domain.SeedWork;

public class IRepository<T> where T : IAggregateRoot
{
    private string _connectionString { get; }

    public IRepository(string connectionString)
        => _connectionString = connectionString;
}