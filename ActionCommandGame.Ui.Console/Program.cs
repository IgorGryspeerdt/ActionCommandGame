using System.Text;
using ActionCommandGame.Sdk;
using ActionCommandGame.Sdk.Extensions;
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
                baseUrl: configuration["ApiBaseUrl"] ?? "https://localhost:5001",
                getToken: () => _jwtToken
            );

           var serviceProvider = serviceCollection.BuildServiceProvider();

            Console.OutputEncoding = Encoding.UTF8;

         
            var authService = serviceProvider.GetRequiredService<AuthSdkService>();
            var playerService = serviceProvider.GetRequiredService<PlayerSdkService>();
            var itemService = serviceProvider.GetRequiredService<ItemSdkService>();
            var gameService = serviceProvider.GetRequiredService<GameSdkService>();

            string? jwtToken = null;
            var authView = new AuthView(authService, token => jwtToken = token);

            while (!await authView.ShowAsync()) { }
         

}
    }
}
