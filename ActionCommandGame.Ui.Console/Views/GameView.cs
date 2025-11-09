using ActionCommandGame.Configuration;
using ActionCommandGame.Sdk;
using ActionCommandGame.Ui.ConsoleApp.Abstractions;
using ActionCommandGame.Ui.ConsoleApp.ConsoleWriters;
using ActionCommandGame.Ui.ConsoleApp.Navigation;
using ActionCommandGame.Ui.ConsoleApp.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActionCommandGame.Dto;

namespace ActionCommandGame.Ui.ConsoleApp.Views
{
    internal class GameView : IView
    {
        private readonly AppSettings _settings;
        private readonly MemoryStore _memoryStore;
        private readonly NavigationManager _navigationManager;
        private readonly GameSdkService _gameService;
        private readonly PlayerSdkService _playerService;
        private readonly ItemSdkService _itemService;

        public GameView(
            AppSettings settings,
            MemoryStore memoryStore,
            NavigationManager navigationManager,
            GameSdkService gameService,
            PlayerSdkService playerService,
            ItemSdkService itemService)
        {
            _settings = settings;
            _memoryStore = memoryStore;
            _navigationManager = navigationManager;
            _gameService = gameService;
            _playerService = playerService;
            _itemService = itemService;
        }

        public async Task Show()
        {
            ConsoleWriter.WriteText($"Play your game. Try typing \"help\" or \"{_settings.ActionCommand}\"", ConsoleColor.Yellow);

            // Player selection
            var players = await _playerService.GetPlayersAsync();
            if (players == null || players.Count == 0)
            {
                Console.WriteLine("No players found. Please create a player first.");
                Console.Write("Enter new player name: ");
                var newName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(newName))
                {
                    Console.WriteLine("Player name cannot be empty.");
                    return;
                }
                var newPlayer = await _playerService.CreatePlayerAsync(newName);
                if (newPlayer == null)
                {
                    Console.WriteLine("Failed to create player.");
                    return;
                }
                players = new List<PlayerDto> { newPlayer };
                Console.WriteLine($"Player '{newPlayer.Name}' created!");
            }

            Console.WriteLine("Select a player:");
            foreach (var p in players)
            {
                Console.WriteLine($"{p.Id}: {p.Name} (Money: {p.Money}, XP: {p.Experience})");
            }
            Console.Write("Enter player id: ");
            int playerId = int.Parse(Console.ReadLine() ?? "0");
            _memoryStore.CurrentPlayerId = playerId;

            while (true)
            {
                ConsoleWriter.WriteText($"{_settings.CommandPromptText} ", ConsoleColor.DarkGray, false);

                string? command = Console.ReadLine();

                Console.Clear();

                if (string.IsNullOrWhiteSpace(command))
                {
                    continue;
                }

                if (CheckCommand(command, new[] { "change player", "player", "change" }))
                {
                    await _navigationManager.NavigateTo<PlayerSelectionView>();
                    break;
                }

                if (CheckCommand(command, new[] { "exit", "quit", "stop" }))
                {
                    await _navigationManager.NavigateTo<ExitView>();
                    break;
                }

                if (CheckCommand(command, new[] { _settings.ActionCommand }))
                {
                    await PerformAction(playerId);
                    await ShowStats(playerId);
                }

                if (CheckCommand(command, new[] { "shop", "store" }))
                {
                    await _navigationManager.NavigateTo<ShopView>();
                }

                if (CheckCommand(command, new[] { "buy", "purchase", "get" }))
                {
                    var itemId = GetIdParameterFromCommand(command);

                    if (!itemId.HasValue)
                    {
                        ConsoleWriter.WriteText("I have no idea what you mean. I have tagged every item with a number. Please give me that number.", ConsoleColor.Red);
                        continue;
                    }

                    await Buy(playerId, itemId.Value);
                }

                if (CheckCommand(command, new[] { "bal", "balance", "money", "xp", "level", "statistics", "stats", "stat", "info" }))
                {
                    await ShowStats(playerId);
                }

                if (CheckCommand(command, new[] { "leaderboard", "lead", "top", "rank", "ranking" }))
                {
                    await _navigationManager.NavigateTo<LeaderboardView>();
                }

                if (CheckCommand(command, new[] { "inventory", "inv", "bag", "backpack" }))
                {
                    await _navigationManager.NavigateTo<InventoryView>();
                }

                if (CheckCommand(command, new[] { "?", "help", "h", "commands" }))
                {
                    await _navigationManager.NavigateTo<HelpView>();
                }
            }
        }

        private static bool CheckCommand(string command, IList<string> matchingCommands)
        {
            return matchingCommands.Any(c => command.ToLower().StartsWith(c.ToLower()));
        }

        public async Task ShowStats(int playerId)
        {
            var player = await _playerService.GetPlayerAsync(playerId);

            if (player == null)
            {
                return;
            }

            // Food
            if (player.CurrentFuelId != null)
            {
                ConsoleWriter.WriteText($"[{player.CurrentFuelName}] ", ConsoleColor.Yellow, false);
                ConsoleWriter.WriteText($"{player.RemainingFuel}/{player.TotalFuel}  ", null, false);
            }
            else
            {
                ConsoleWriter.WriteText("[Food] ", ConsoleColor.Red, false);
                ConsoleWriter.WriteText("nothing ", null, false);
            }

            // Attack
            if (player.CurrentAttackId != null)
            {
                ConsoleWriter.WriteText($"[{player.CurrentAttackName}] ", ConsoleColor.Yellow, false);
                ConsoleWriter.WriteText($"{player.RemainingAttack}/{player.TotalAttack}  ", null, false);
            }
            else
            {
                ConsoleWriter.WriteText("[Attack] ", ConsoleColor.Red, false);
                ConsoleWriter.WriteText("nothing ", null, false);
            }

            // Defense
            if (player.CurrentDefenseId != null)
            {
                ConsoleWriter.WriteText($"[{player.CurrentDefenseName}] ", ConsoleColor.Yellow, false);
                ConsoleWriter.WriteText($"{player.RemainingDefense}/{player.TotalDefense}  ", null, false);
            }
            else
            {
                ConsoleWriter.WriteText("[Defense] ", ConsoleColor.Red, false);
                ConsoleWriter.WriteText("nothing ", null, false);
            }

            ConsoleWriter.WriteText("[Money] ", ConsoleColor.Yellow, false);
            ConsoleWriter.WriteText($"€{player.Money}  ", null, false);

            // Level calculation
            int level = CalculateLevel(player.Experience);
            int nextLevelXp = GetExperienceForNextLevel(level);
            ConsoleWriter.WriteText("[Level] ", ConsoleColor.Yellow, false);
            ConsoleWriter.WriteText($"{level} ({player.Experience}/{nextLevelXp})  ", null, false);

            ConsoleWriter.WriteText();
            ConsoleWriter.WriteText();
        }

        private async Task PerformAction(int playerId)
        {
            var result = await _gameService.PerformActionAsync(playerId);

            if (result == null || result.Player == null)
            {
                return;
            }

            var player = result.Player;
            var positiveGameEvent = result.PositiveGameEvent;
            var negativeGameEvent = result.NegativeGameEvent;

            if (positiveGameEvent != null)
            {
                ConsoleWriter.WriteText($"{string.Format(_settings.ActionText, player.Name)} ",
                    ConsoleColor.Green, false);
                ConsoleWriter.WriteText(positiveGameEvent.Name, ConsoleColor.White);
                if (!string.IsNullOrWhiteSpace(positiveGameEvent.Description))
                {
                    ConsoleWriter.WriteText(positiveGameEvent.Description);
                }
                if (positiveGameEvent.Money > 0)
                {
                    ConsoleWriter.WriteText($"€{positiveGameEvent.Money}", ConsoleColor.Yellow, false);
                    ConsoleWriter.WriteText(" has been added to your account.");
                }
            }

            if (negativeGameEvent != null)
            {
                ConsoleWriter.WriteText(negativeGameEvent.Name, ConsoleColor.Blue);
                if (!string.IsNullOrWhiteSpace(negativeGameEvent.Description))
                {
                    ConsoleWriter.WriteText(negativeGameEvent.Description);
                }
                if (result.NegativeGameEventMessages != null)
                {
                    foreach (var msg in result.NegativeGameEventMessages)
                    {
                        ConsoleWriter.WriteText(msg, ConsoleColor.Red);
                    }
                }
            }

            ConsoleWriter.WriteText();
        }

        private async Task Buy(int playerId, int itemId)
        {
            var result = await _itemService.BuyItemAsync(playerId, itemId);

            if (result != null && result.Item != null)
            {
                ConsoleWriter.WriteText($"You bought {result.Item.Name} for €{result.Item.Price}");
                ConsoleWriter.WriteText($"Thank you for shopping. Your current balance is €{result.Player.Money}.");
            }
            else
            {
                ConsoleWriter.WriteText("Purchase failed.", ConsoleColor.Red);
            }

            Console.WriteLine();
        }

        private int? GetIdParameterFromCommand(string command)
        {
            var commandParts = command.Split(" ");
            if (commandParts.Length == 1)
            {
                return null;
            }
            var idPart = commandParts[1];

            int.TryParse(idPart, out var itemId);

            return itemId;
        }

        private int CalculateLevel(int experience)
        {
            return (int)((Math.Sqrt(100 * (2 * experience + 25)) + 50) / 100);
        }

        private int GetExperienceForNextLevel(int level)
        {
            return ((level * level + level) / 2 * 100) - (level * 100);
        }
    }
}
