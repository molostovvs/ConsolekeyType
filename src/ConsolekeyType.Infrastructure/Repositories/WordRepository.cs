using ConsolekeyType.Domain.Aggregates.TypingTestAggregate;
using Microsoft.Extensions.Options;

namespace ConsolekeyType.Infrastructure.Repositories;

public class WordRepository : IWordRepository
{
    private readonly string _connectionString;

    public WordRepository(IOptions<DatabaseSettings> dbSettings)
        => _connectionString = dbSettings.Value.ConnectionString;

    public Result<IReadOnlyList<Word>> Get(int count, Language language)
    {
        using var connection = new SQLiteConnection(_connectionString);
        using var command = new SQLiteCommand(connection);
        command.CommandText = @"
select
    word
from
    words
where
    language_id = @language_id
order by random()
limit @count
";
        command.Parameters.AddWithValue("@language_id", language.Id);
        command.Parameters.AddWithValue("@count", count);

        var result = new List<Word>();

        connection.Open();
        using var reader = command.ExecuteReader(); //returns null if there is no Words table

        while (reader.Read())
        {
            var word = (Word)(string)reader["word"];
            result.Add(word);
        }

        return result;
    }
}