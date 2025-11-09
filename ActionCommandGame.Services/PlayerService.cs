using ActionCommandGame.Model;
using ActionCommandGame.Repository;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Extensions;
using ActionCommandGame.Services.Extensions.Filters;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Filters;
using ActionCommandGame.Services.Model.Results;
using ActionCommandGame.Services.Model.Requests;
using Microsoft.EntityFrameworkCore;

namespace ActionCommandGame.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly ActionCommandGameDbContext _database;

        public PlayerService(ActionCommandGameDbContext database)
        {
            _database = database;
        }

        public async Task<ServiceResult<PlayerResult>> Get(int id, int userId)
        {
            var player = await _database.Players
                .Where(p => p.Id == id && p.UserId == userId)
                .ProjectToResult()
                .SingleOrDefaultAsync();

            return new ServiceResult<PlayerResult>(player);
        }

        public async Task<ServiceResult<IList<PlayerResult>>> Find(PlayerFilter? filter)
        {
            var query = _database.Players.AsQueryable();
            if (filter?.UserId != null)
            {
                query = query.Where(p => p.UserId == filter.UserId);
            }
            var players = await query
                .ApplyFilter(filter)
                .ProjectToResult()
                .ToListAsync();

            return new ServiceResult<IList<PlayerResult>>(players);
        }
        public async Task<ServiceResult<PlayerResult>> Create(string name, int userId)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return new ServiceResult<PlayerResult>
                {
                    Messages = new List<ServiceMessage>
                    {
                        new ServiceMessage { Code = "InvalidName", Message = "Player name is required." }
                    }
                };
            }

            var player = new Player
            {
                Name = name,
                UserId = userId,
                Money = 200,
                Experience = 0,
                LastActionExecutedDateTime = null,
                Inventory = new List<PlayerItem>()
            };

            _database.Players.Add(player);
            await _database.SaveChangesAsync();

            var playerResult = await _database.Players
                .ProjectToResult()
                .SingleOrDefaultAsync(p => p.Id == player.Id);

            return new ServiceResult<PlayerResult>(playerResult);
        }

        public async Task<ServiceResult<PlayerResult>> Update(UpdatePlayerRequest model, int userId)
        {
            var player = await _database.Players
                .Where(p => p.Id == model.Id && p.UserId == userId)
                .FirstOrDefaultAsync();

            if (player == null)
            {
                return new ServiceResult<PlayerResult>
                {
                    Messages = new List<ServiceMessage>
                    {
                        new ServiceMessage { Code = "NotFound", Message = "Player not found." }
                    }
                };
            }

            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                player.Name = model.Name;
            }

            // Add other updatable fields as needed

            await _database.SaveChangesAsync();

            var playerResult = await _database.Players
                .ProjectToResult()
                .SingleOrDefaultAsync(p => p.Id == player.Id);

            return new ServiceResult<PlayerResult>(playerResult);
        }

        public async Task<ServiceResult> Delete(int id, int userId)
        {
            var player = await _database.Players
                .Where(p => p.Id == id && p.UserId == userId)
                .FirstOrDefaultAsync();

            if (player == null)
            {
                return new ServiceResult
                {
                    Messages = new List<ServiceMessage>
                    {
                        new ServiceMessage { Code = "NotFound", Message = "Player not found." }
                    }
                };
            }

            _database.Players.Remove(player);
            await _database.SaveChangesAsync();

            return new ServiceResult();
        }
    }
}
