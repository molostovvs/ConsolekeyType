//TODO: should this project be called Consolekey.Application instead of Consolekey.UI? If so, new project with actual UI should be introduced

using Microsoft.Extensions.DependencyInjection;
using ConsolekeyType.Infrastructure;
using ConsolekeyType.Domain.Aggregates.TypingTestAggregate;
using Microsoft.Extensions.Configuration;

IConfiguration configuration =
    new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();

var services = new ServiceCollection();
ConfigureServices(services, configuration);
var serviceProvider = services.BuildServiceProvider();

try
{
    // UI.Run();
}
catch (Exception e)
{
    // _Logger.Log(e);
    //UI.ShowApology();
    //UI.FailFast();
}

static void ConfigureServices(IServiceCollection serviceCollection, IConfiguration configuration)
{
    serviceCollection.AddOptions<DatabaseSettings>()
                     .Bind(configuration.GetSection(DatabaseSettings.ConfigKey));

    serviceCollection.AddScoped<ITypingTestRepository, TypingTestRepository>();
}