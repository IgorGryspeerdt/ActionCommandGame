using ActionCommandGame.Dto;
using ActionCommandGame.Sdk;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ActionCommandGame.Services.Abstractions;

namespace ActionCommandGame.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ActionController : ControllerBase
    {
        private readonly IGameService _gameService;

        public ActionController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpPost("perform")]
        public async Task<ActionResult<GameResultDto>> PerformAction([FromBody] PerformActionRequest model)
        {
            var result = await _gameService.PerformAction(model.PlayerId);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        public class PerformActionRequest
        {
            public int PlayerId { get; set; }
        }
    }
}
