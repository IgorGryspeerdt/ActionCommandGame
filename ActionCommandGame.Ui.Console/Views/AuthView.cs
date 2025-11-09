using ActionCommandGame.Sdk;
using ActionCommandGame.Ui.ConsoleApp.Stores;
using System;
using System.Threading.Tasks;

namespace ActionCommandGame.Ui.ConsoleApp.Views
{
    public class AuthView
    {
        private readonly AuthSdkService _authService;
        private readonly Action<string> _setToken;

        public AuthView(AuthSdkService authService, Action<string> setToken)
        {
            _authService = authService;
            _setToken = setToken;
        }


        public async Task<bool> ShowAsync()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Action Command Game!");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Register");
            Console.Write("Choose option (1 or 2): ");
            var choice = Console.ReadLine();

            Console.Write("Email: ");
            var email = Console.ReadLine();
            Console.Write("Password: ");
            var password = ReadPassword();

            string? token = null;
            if (choice == "1")
            {
                token = await _authService.LoginAsync(email, password);
            }
            else if (choice == "2")
            {
                token = await _authService.RegisterAsync(email, password);
            }
            if (!string.IsNullOrEmpty(token))
            {
                _setToken(token);
                Console.WriteLine("Authentication successful! Press any key to continue...");
                Console.ReadKey();
                return true;
            }
            else
            {
                Console.WriteLine("Authentication failed. Press any key to try again...");
                Console.ReadKey();
                return false;
            }
        }

        private static string ReadPassword()
        {
            var password = string.Empty;
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password[0..^1];
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    password += keyInfo.KeyChar;
                    Console.Write("*");
                }
            } while (key != ConsoleKey.Enter);
            Console.WriteLine();
            return password;
        }
    }
}