using System.Text;
using ActionCommandGame.Configuration;
using ActionCommandGame.Sdk;
using ActionCommandGame.Sdk.Extensions;
using ActionCommandGame.Services.Abstractions;
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

        public class MyService
        {
            public MyService(HttpClient client)
            {
                Console.WriteLine($"[DI DEBUG] MyService constructed. BaseAddress: {client.BaseAddress}");
            }
        }

        static async Task Main()
        {
            //var testServices = new ServiceCollection();
            //testServices.AddHttpClient<MyService>(c => c.BaseAddress = new Uri("https://localhost:7213"));
            //var testProvider = testServices.BuildServiceProvider();
            //var testSvc = testProvider.GetRequiredService<MyService>();


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

            // Register NavigationManager with IServiceProvider
            serviceCollection.AddSingleton<NavigationManager>(provider =>
                new NavigationManager(provider));

            // Register your views
            serviceCollection.AddTransient<HelpView>();
            serviceCollection.AddTransient<ExitView>();
            serviceCollection.AddTransient<TitleView>();
            serviceCollection.AddTransient<AuthView>();
            serviceCollection.AddTransient<InventoryView>();
            serviceCollection.AddTransient<LeaderboardView>();
            serviceCollection.AddTransient<ShopView>();
            serviceCollection.AddTransient<PlayerSelectionView>();
            serviceCollection.AddTransient<GameView>();


            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Register NavigationManager with the root provider
            var navigationManager = new NavigationManager(serviceProvider);
            serviceCollection.AddSingleton(navigationManager);

            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine($"[DI DEBUG] Main IServiceProvider HashCode: {serviceProvider.GetHashCode()}");

            var authService = serviceProvider.GetRequiredService<AuthSdkService>();
            var playerService = serviceProvider.GetRequiredService<IPlayerService>();
            var itemService = serviceProvider.GetRequiredService<IItemService>();
            var gameService = serviceProvider.GetRequiredService<GameSdkService>();

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