using ConsolekeyType.Domain.Aggregates.TypingTestAggregate;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;

namespace ConsolekeyType.Infrastructure;

public class TypingTestRepository : ITypingTestRepository
{
    private readonly string _connectionString;

    public TypingTestRepository(IOptions<DatabaseSettings> dbSettings)
        => _connectionString = dbSettings.Value.ConnectionString;

    public Result Save(TypingTest typingTest)
        => throw new NotImplementedException();

    public Result<IEnumerable<TypingTest>> GetAll()
        => throw new NotImplementedException();

    public Maybe<TypingTest> GetById(long id)
        => throw new NotImplementedException();
}