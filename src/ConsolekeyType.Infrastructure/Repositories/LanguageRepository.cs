using ConsolekeyType.Domain.Aggregates.TypingTestAggregate;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;

namespace ConsolekeyType.Infrastructure.Repositories;

public class LanguageRepository : ILanguageRepository
{
    private readonly string _connectionString;

    public LanguageRepository(IOptions<DatabaseSettings> dbSettings)
        => _connectionString = dbSettings.Value.ConnectionString;

    public Result<IEnumerable<Language>> GetAll()
        => throw new NotImplementedException();

    public Maybe<Language> GetById(long id)
        => throw new NotImplementedException();

    public Maybe<Language> GeByName(string name)
        => throw new NotImplementedException();
}