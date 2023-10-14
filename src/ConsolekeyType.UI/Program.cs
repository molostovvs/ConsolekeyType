using ConsolekeyType.Application;
using Microsoft.Extensions.DependencyInjection;
using ConsolekeyType.Infrastructure;
using ConsolekeyType.Domain.Aggregates.TypingTestAggregate;
using ConsolekeyType.Infrastructure.Repositories;
using ConsolekeyType.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false)
                                       .AddJsonFile(
                                            $"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}.json",
                                            optional: true
                                        )
                                       .Build();

var services = new ServiceCollection();
ConfigureServices(services, config);
var provider = services.BuildServiceProvider();

var db = provider.GetRequiredService<IDatabaseHandler>();
db.Initialize();

var ui = new TypingTestUI(
    provider.GetRequiredService<ITypingTestService>(),
    provider.GetRequiredService<ITextService>()
);

Console.CancelKeyPress += delegate
{
    ConsoleHelper.SetCursorMin();
    ConsoleHelper.Clear();
};

// try
// {
    ui.Run();
// }
// catch (Exception)
// {
//     ui.ShowApology();
//     ui.FailFast();
// }

void ConfigureServices(IServiceCollection serviceCollection, IConfiguration configuration)
{
    serviceCollection.AddOptions<DatabaseSettings>()
                     .Bind(configuration.GetSection(DatabaseSettings.ConfigKey));

    serviceCollection.AddScoped<ITypingTestRepository, TypingTestRepository>();
    serviceCollection.AddScoped<IWordRepository, WordRepository>();
    serviceCollection.AddScoped<ILanguageRepository, LanguageRepository>();
    serviceCollection.AddTransient<ITypingTestService, TypingTestService>();
    serviceCollection.AddTransient<ITextService, TextService>();
    serviceCollection.AddSingleton<IDatabaseHandler, DatabaseHandler>();
}