namespace ConsolekeyType.Domain.SeedWork;

public interface IRepository<T> where T : IAggregateRoot
{
    private protected string _connectionString { get; }
}