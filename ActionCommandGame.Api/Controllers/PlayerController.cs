using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Filters;
using ActionCommandGame.Services.Model.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ActionCommandGame.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        [HttpGet]
        public async Task<IActionResult> GetPlayers()
        {
            var userId = GetCurrentUserId();
            var result = await _playerService.Find(new PlayerFilter { UserId = userId });
            if (result.Data == null)
            {
                return NotFound(result.Messages);
            }
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayer(int id)
        {
            var userId = GetCurrentUserId();
            var result = await _playerService.Get(id, userId);
            if (result.Data == null)
            {
                return NotFound(result.Messages);
            }
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlayer([FromBody] CreatePlayerRequest model)
        {
            var userId = GetCurrentUserId();
            var result = await _playerService.Create(model.Name, userId);
            if (result.Data == null)
            {
                return BadRequest(result.Messages);
            }
            return CreatedAtAction(nameof(GetPlayer), new { id = result.Data.Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlayer(int id, [FromBody] UpdatePlayerRequest model)
        {
            var userId = GetCurrentUserId();
            if (id != model.Id)
            {
                return BadRequest("Player ID mismatch.");
            }
            var result = await _playerService.Update(model, userId);
            if (result.Data == null)
            {
                return BadRequest(result.Messages);
            }
            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var userId = GetCurrentUserId();
            var result = await _playerService.Delete(id, userId);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Messages);
            }
            return NoContent();
        }
    }
}
