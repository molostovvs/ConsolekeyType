using Microsoft.Extensions.Options;

namespace ConsolekeyType.Infrastructure;

public class DatabaseHandler : IDatabaseHandler
{
    private readonly string _connectionString;
    private readonly string _dbName;

    public DatabaseHandler(IOptions<DatabaseSettings> dbSettings)
    {
        _connectionString = dbSettings.Value.ConnectionString;
        _dbName = _connectionString.Split(';', '=')[1].Trim();
    }

    public void Initialize()
    {
        if (File.Exists(_dbName))
            return;

        var scripts = GetScripts();
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();
        foreach (var script in scripts)
            ExecuteSqlScript(script, connection);
    }

    public void Terminate()
    {
        if (File.Exists(_dbName))
            File.Delete(_dbName);
    }

    private static List<string> GetScripts()
        => Directory.GetFiles("Migrations/", "*.SQL")
                    .Where(file => file != "Migrations/GlobalReset.SQL")
                    .OrderBy(file => file)
                    .Select(File.ReadAllText)
                    .ToList();

    private static void ExecuteSqlScript(string script, SQLiteConnection connection)
    {
        var command = new SQLiteCommand(connection);
        command.CommandText = script;
        command.ExecuteNonQuery();
    }
}