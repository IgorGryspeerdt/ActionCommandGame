using ActionCommandGame.Dto;
using ActionCommandGame.Sdk;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ActionCommandGame.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ActionController : ControllerBase
    {
        private readonly GameSdkService _gameSdkService;

        public ActionController(GameSdkService gameSdkService)
        {
            _gameSdkService = gameSdkService;
        }

        [HttpPost("perform")]
        public async Task<ActionResult<GameResultDto>> PerformAction([FromBody] PerformActionRequest model)
        {
            var result = await _gameSdkService.PerformActionAsync(model.PlayerId);
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
