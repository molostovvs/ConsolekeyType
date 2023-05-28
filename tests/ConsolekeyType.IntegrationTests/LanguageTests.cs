using System.Data.SQLite;
using System.Reflection;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;

namespace ConsolekeyType.IntegrationTests;

[TestFixture]
public class LanguageTests
{
    [SetUp]
    public void SetUp()
    {
        using var connection = new SQLiteConnection(_connectionString);

        using var createTable = new SQLiteCommand(
            File.ReadAllText("Migrations/" + "202305261744-CreateLanguage.SQL"),
            connection
        );

        using var insertData = new SQLiteCommand(
            File.ReadAllText("Migrations/" + "202305261746-InsertLanguages.SQL"),
            connection
        );

        connection.Open();
        createTable.ExecuteNonQuery();
        insertData.ExecuteNonQuery();
    }

    [TearDown]
    public void TearDown()
        => File.Delete("test.db");

    [Test]
    public void All_required_values_in_db_by_id()
    {
        foreach (var language in Language.All)
            Verify(language, (lang, repo) => repo.GetById(lang.Id));
    }

    [Test]
    public void All_required_values_in_db_by_name()
    {
        foreach (var language in Language.All)
            Verify(language, (lang, repo) => repo.GetByName(lang.Name));
    }

    private void Verify(Language hardcodedLanguage,
        Func<Language, LanguageRepository, Maybe<Language>> languageRetriever)
    {
        var repository = new LanguageRepository(GetOptions());
        var languageOrNothing = languageRetriever(hardcodedLanguage, repository);

        if (languageOrNothing.HasNoValue)
            Assert.Fail();

        var languageFromDb = languageOrNothing.Value;

        languageFromDb.Id.Should().Be(hardcodedLanguage.Id);
        languageFromDb.Name.Should().Be(hardcodedLanguage.Name);
    }

    private const string _connectionString =
        "data source = test.db;version = 3;failifmissing = false";

    private static IOptions<DatabaseSettings> GetOptions()
        => Options.Create(
            new DatabaseSettings
            {
                ConnectionString = _connectionString
            }
        );
}