using ActionCommandGame.Dto;
using ActionCommandGame.Sdk;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Filters;
using ActionCommandGame.Services.Model.Requests;
using CreatePlayerRequest = ActionCommandGame.Dto.requests.CreatePlayerRequest;

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

        private int GetUserId()
        {
            return int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
        }

        [HttpGet]
        public async Task<ActionResult<List<PlayerDto>>> GetPlayers()
        {
            var userId = GetUserId();
            var filter = new PlayerFilter { FilterUserPlayers = true, UserId = userId };
            var result = await _playerService.Find(filter);
            if (!result.IsSuccess || result.Data == null || result.Data.Count == 0)
            {
                return NotFound();
            }
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerDto>> GetPlayer(int id)
        {
            var userId = GetUserId();
            var result = await _playerService.Get(id, userId);
            if (!result.IsSuccess || result.Data == null)
            {
                return NotFound();
            }
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<ActionResult<PlayerDto>> CreatePlayer([FromBody] CreatePlayerRequest model)
        {
            var userId = GetUserId();
            var result = await _playerService.Create(model.Name, userId);
            if (!result.IsSuccess || result.Data == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetPlayer), new { id = result.Data.Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PlayerDto>> UpdatePlayer(int id, [FromBody] UpdatePlayerRequest model)
        {
            if (id != model.Id)
            {
                return BadRequest("Player ID mismatch.");
            }
            var userId = GetUserId();

            // Map DTO to service model
            var serviceModel = new Services.Model.Requests.UpdatePlayerRequest
            {
                Id = model.Id,
                Name = model.Name
                // Map other properties if needed
            };

            var result = await _playerService.Update(serviceModel, userId);
            if (!result.IsSuccess || result.Data == null)
            {
                return BadRequest();
            }
            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var userId = GetUserId();
            var result = await _playerService.Delete(id, userId);
            if (!result.IsSuccess)
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}
