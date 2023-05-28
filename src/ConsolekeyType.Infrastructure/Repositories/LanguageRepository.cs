using ConsolekeyType.Domain.Aggregates.TypingTestAggregate;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace ConsolekeyType.Infrastructure.Repositories;

public class LanguageRepository : ILanguageRepository
{
    private readonly string _connectionString;

    public LanguageRepository(IOptions<DatabaseSettings> dbSettings)
        => _connectionString = dbSettings.Value.ConnectionString;

    public Maybe<Language> GetById(long id)
    {
        using var connection = new SQLiteConnection(_connectionString);

        using var command = new SQLiteCommand(connection);
        command.CommandText = @"
select
    id,
    name
from
    language
where id == @id";
        command.Parameters.AddWithValue("@id", id);

        connection.Open();

        using var reader = command.ExecuteReader();
        reader.Read();
        return Map(reader);
    }

    public Maybe<Language> GetByName(string name)
    {
        using var connection = new SQLiteConnection(_connectionString);

        using var command = new SQLiteCommand(connection);
        command.CommandText = @"
select
    id,
    name
from
    language
where name == @name";
        command.Parameters.AddWithValue("@name", name);

        connection.Open();

        using var reader = command.ExecuteReader();
        reader.Read();
        return Map(reader);
    }

    private Maybe<Language> Map(IDataRecord reader)
    {
        var ctor = typeof(Language).GetConstructor(
            BindingFlags.Instance | BindingFlags.NonPublic,
            null,
            CallingConventions.HasThis,
            new[] { typeof(int), typeof(string) },
            null
        );

        return (Language)ctor.Invoke(new[] { reader["id"], reader["name"] });
    }
}