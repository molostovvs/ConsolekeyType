namespace ConsolekeyType.Infrastructure;

public class DatabaseSettings
{
    public const string ConfigKey = nameof(DatabaseSettings);

    public string ConnectionString { get; set; }
}