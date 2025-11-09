using ActionCommandGame.Ui.ConsoleApp.Abstractions;
using ActionCommandGame.Ui.ConsoleApp.ConsoleWriters;
using Microsoft.Extensions.DependencyInjection;

namespace ActionCommandGame.Ui.ConsoleApp.Navigation
{
    public class NavigationManager
    {
        private readonly IServiceProvider _serviceProvider;

        public NavigationManager(IServiceProvider serviceProvider)
        {
            Console.WriteLine($"[DI DEBUG] NavigationManager IServiceProvider HashCode: {serviceProvider.GetHashCode()}");
            _serviceProvider = serviceProvider;
        }

        public async Task NavigateTo<T>(bool clearScreen = true) where T : IView
        {
            Console.WriteLine($"[DI DEBUG] IServiceProvider HashCode: {_serviceProvider.GetHashCode()}");

            var view = _serviceProvider.GetRequiredService<T>();
            if (view is null)
            {
                ConsoleWriter.WriteText("Error navigating to view", ConsoleColor.Red);
                return;
            }

            if (clearScreen)
            {
                Console.Clear();
            }
            await view.Show();
        }
    }
}
