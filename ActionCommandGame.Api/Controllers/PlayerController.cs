using ActionCommandGame.Dto;
using ActionCommandGame.Sdk;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using ActionCommandGame.Dto.requests;

namespace ActionCommandGame.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly PlayerSdkService _playerSdkService;

        public PlayerController(PlayerSdkService playerSdkService)
        {
            _playerSdkService = playerSdkService;
        }

        [HttpGet]
        public async Task<ActionResult<List<PlayerDto>>> GetPlayers()
        {
            var players = await _playerSdkService.GetPlayersAsync();
            if (players == null || players.Count == 0)
            {
                return NotFound();
            }
            return Ok(players);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerDto>> GetPlayer(int id)
        {
            var player = await _playerSdkService.GetPlayerAsync(id);
            if (player == null)
            {
                return NotFound();
            }
            return Ok(player);
        }

        [HttpPost]
        public async Task<ActionResult<PlayerDto>> CreatePlayer([FromBody] CreatePlayerRequest model)
        {
            var player = await _playerSdkService.CreatePlayerAsync(model.Name);
            if (player == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetPlayer), new { id = player.Id }, player);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PlayerDto>> UpdatePlayer(int id, [FromBody] UpdatePlayerRequest model)
        {
            if (id != model.Id)
            {
                return BadRequest("Player ID mismatch.");
            }
            var player = await _playerSdkService.UpdatePlayerAsync(model);
            if (player == null)
            {
                return BadRequest();
            }
            return Ok(player);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var success = await _playerSdkService.DeletePlayerAsync(id);
            if (!success)
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}
