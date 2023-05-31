using System.Data.SQLite;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using CSharpFunctionalExtensions.FluentAssertions;

namespace ConsolekeyType.IntegrationTests;

[TestFixture]
public class TypingTestTests
{
    private const string _connectionString =
        "data source = test.db;version = 3;failifmissing = false";

    private readonly DateTime _startTime = new(2020, 1, 1, 10, 0, 0, 50);
    private readonly DateTime _endTime = new(2020, 1, 1, 10, 0, 2, 440);

    [SetUp]
    public void SetUp()
    {
        if (File.Exists("test.db"))
            File.Delete("test.db");

        using var connection = new SQLiteConnection(_connectionString);
        using var createTable = new SQLiteCommand(
            File.ReadAllText("Migrations/" + "202305290837-CreateTypingTests.SQL"),
            connection
        );

        connection.Open();
        createTable.ExecuteNonQuery();
    }

    [TearDown]
    public void TearDown()
        => File.Delete("test.db");

    [Test]
    public void Saving_to_db()
    {
        var result = SaveDefaultTypingTestToDatabase();

        result.Should().Succeed();
    }

    [Test]
    public void Getting_by_id()
    {
        var savedTypingTest = SaveDefaultTypingTestToDatabase().Value;
        var repo = new TypingTestRepository(GetOptions());
        const long id = 1;
        var retrievedTypingTest = repo.GetById(id).Value;

        retrievedTypingTest.Id.Should().Be(id);
        retrievedTypingTest.Text.Should().Be(savedTypingTest.Text);
        retrievedTypingTest.StartTime.Should().Be(savedTypingTest.StartTime);
        retrievedTypingTest.EndTime.Should().Be(savedTypingTest.EndTime);
        retrievedTypingTest.Duration.Value.Should().Be(savedTypingTest.Duration.Value);
    }

    private Result<TypingTest> SaveDefaultTypingTestToDatabase()
    {
        var text = Text.Create("ponchi is a fluffy corgi pembroke", Language.English);
        var typingTest = TypingTest.Create(text.Value).Value;
        typingTest.Start(_startTime);
        typingTest.End(_endTime);

        var repo = new TypingTestRepository(GetOptions());
        var result = repo.Save(typingTest);

        if (result.IsSuccess)
            return Result.Success(typingTest);

        return Result.Failure<TypingTest>("Cannot connect to db");
    }

    private static IOptions<DatabaseSettings> GetOptions()
        => Options.Create(
            new DatabaseSettings
            {
                ConnectionString = _connectionString
            }
        );
}