using System.Text;
using ActionCommandGame.Configuration;
using ActionCommandGame.Sdk;
using ActionCommandGame.Sdk.Extensions;
using ActionCommandGame.Ui.ConsoleApp.Navigation;
using ActionCommandGame.Ui.ConsoleApp.Stores;
using ActionCommandGame.Ui.ConsoleApp.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ActionCommandGame.Ui.ConsoleApp
{
    class Program
    {
        private static string? _jwtToken;

        static async Task Main()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var configuration = builder.Build();

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddActionCommandGameSdk(
                baseUrl: configuration["ApiBaseUrl"] ?? "https://localhost:7213",
                getToken: () => _jwtToken
            );

            serviceCollection.AddSingleton<AppSettings>(provider =>
                configuration.GetSection("AppSettings").Get<AppSettings>());
            serviceCollection.AddSingleton<MemoryStore>();
            serviceCollection.AddSingleton<NavigationManager>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            Console.OutputEncoding = Encoding.UTF8;

            var authService = serviceProvider.GetRequiredService<AuthSdkService>();
            var playerService = serviceProvider.GetRequiredService<PlayerSdkService>();
            var itemService = serviceProvider.GetRequiredService<ItemSdkService>();
            var gameService = serviceProvider.GetRequiredService<GameSdkService>();

            var navigationManager = serviceProvider.GetService<NavigationManager>();
            var memoryStore = serviceProvider.GetService<MemoryStore>();
            var appSettings = serviceProvider.GetService<AppSettings>();

            string? jwtToken = null;
            var authView = new AuthView(authService, token => _jwtToken = token);

            while (!await authView.ShowAsync()) { }

            // Start the main game view after authentication
            var gameView = new GameView(
                appSettings!,
                memoryStore!,
                navigationManager!,
                gameService,
                playerService,
                itemService);

            await gameView.Show();
        }
    }
}
