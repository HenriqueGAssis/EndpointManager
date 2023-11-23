using EndpointManager.Helper;
using EndpointManager.Service;
using EndpointManager.UserInterface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

class Program
{
    internal static IServiceProvider ServiceProvider;

    private static IMenu _menu;
    private static ILogger _logger;

    static void Main(string[] args)
    {
        Config();

        Run();
    }

    private static void Run()
    {
        _menu.ShowUserInteraction();
    }

    private static void Config()
    {
        ServiceProvider = new ServiceCollection()
            .AddLogging(loggingBuilder =>
            {
                loggingBuilder.SetMinimumLevel(LogLevel.Debug);
                loggingBuilder.AddConsole();
            })
            .AddSingleton<IEndpointService, EndpointService>()
            .AddSingleton<IMenu, Menu>()
            .AddSingleton<IUserInput, UserInput>()
            .AddSingleton<ICacheService, CacheService>()
            .AddSingleton<ILoggerService, LoggerService>()
            .AddMemoryCache()
            .BuildServiceProvider();

        _menu = ServiceProvider.GetService<IMenu>();
        _logger = ServiceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();
    }
}