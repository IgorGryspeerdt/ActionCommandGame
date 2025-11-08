using ActionCommandGame.Repository;
using ActionCommandGame.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ActionCommandGame.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly IGameService _gameService;
        private readonly IPlayerService _playerService;

        public ItemController(IItemService itemService, IGameService gameService, IPlayerService playerService)
        {
            _itemService = itemService;
            _gameService = gameService;
            _playerService = playerService;
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        
        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            var result = await _itemService.Find();
            if (result.Data == null)
            {
                return NotFound(result.Messages);
            }
            return Ok(result.Data);
        }

        [HttpPost("buy")]
        public async Task<IActionResult> BuyItem([FromBody] BuyItemRequest model)
        {
            var userId = GetCurrentUserId();
            // Check player ownership
            var playerResult = await _playerService.Get(model.PlayerId, userId);
            if (playerResult.Data == null)
            {
                return Forbid("You do not own this player or it does not exist.");
            }

            var result = await _gameService.Buy(model.PlayerId, model.ItemId);
            if (result.Data == null)
            {
                return BadRequest(result.Messages);
            }
            return Ok(result.Data);
        }

        public class BuyItemRequest
        {
            public int PlayerId { get; set; }
            public int ItemId { get; set; }
        }
    }
}
