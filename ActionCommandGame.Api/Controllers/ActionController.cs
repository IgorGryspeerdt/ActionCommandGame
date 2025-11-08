using ActionCommandGame.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ActionCommandGame.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ActionController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly IPlayerService _playerService;

        public ActionController(IGameService gameService, IPlayerService playerService)
        {
            _gameService = gameService;
            _playerService = playerService;
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        [HttpPost("perform")]
        public async Task<IActionResult> PerformAction([FromBody] PerformActionRequest model)
        {
            var userId = GetCurrentUserId();
            
            var playerResult = await _playerService.Get(model.PlayerId, userId);
            if (playerResult.Data == null)
            {
                return Forbid("You do not own this player or it does not exist.");
            }

            var result = await _gameService.PerformAction(model.PlayerId);

            if (result.Data == null)
            {
                
                return BadRequest(result.Messages);
            }

            return Ok(result);
        }

        public class PerformActionRequest
        {
            public int PlayerId { get; set; }
        }
    }
}
