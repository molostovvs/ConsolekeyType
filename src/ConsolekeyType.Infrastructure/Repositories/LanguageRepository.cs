using System.Data;
using System.Data.Common;
using ConsolekeyType.Domain.Aggregates.TypingTestAggregate;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using System.Data.SQLite;
using System.Reflection;

namespace ConsolekeyType.Infrastructure.Repositories;

public class LanguageRepository : ILanguageRepository
{
    private readonly string _connectionString;

    public LanguageRepository(IOptions<DatabaseSettings> dbSettings)
        => _connectionString = dbSettings.Value.ConnectionString;

    public Maybe<Language> GetById(long id)
    {
        var result = Maybe<Language>.None;

        using (var connection = new SQLiteConnection(_connectionString))
        {
            var command = connection.CreateCommand();
            command.CommandText = @"
select
    id,
    name
from
    language
where id == @id";

            command.Parameters.AddWithValue("@id", id);

            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                reader.Read();
                Map(reader, ref result);
            }

            connection.Close();
        }

        return result;
    }

    private void Map(DbDataReader reader, ref Maybe<Language> result)
    {
        var ctor = typeof(Language).GetConstructor(
            BindingFlags.Instance | BindingFlags.NonPublic,
            null,
            CallingConventions.HasThis,
            new[] { typeof(int), typeof(string) },
            null
        );

        result = (Language)ctor.Invoke(new object[] { reader["id"], reader["name"] });
    }

    public Maybe<Language> GetByName(string name)
    {
        var result = Maybe<Language>.None;

        using (var connection = new SQLiteConnection(_connectionString))
        {
            var command = connection.CreateCommand();
            command.CommandText = @"
select
    id,
    name
from
    language
where name == @name";

            command.Parameters.AddWithValue("@name", name);

            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                reader.Read();
                Map(reader, ref result);
            }

            connection.Close();
        }

        return result;
    }
}