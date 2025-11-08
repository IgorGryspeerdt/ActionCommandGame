using ActionCommandGame.Dto;
using ActionCommandGame.Sdk;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ActionCommandGame.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly ItemSdkService _itemSdkService;

        public ItemController(ItemSdkService itemSdkService)
        {
            _itemSdkService = itemSdkService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ItemDto>>> GetItems()
        {
            var items = await _itemSdkService.GetItemsAsync();
            if (items == null || items.Count == 0)
            {
                return NotFound();
            }
            return Ok(items);
        }

        [HttpPost("buy")]
        public async Task<ActionResult<BuyResultDto>> BuyItem([FromBody] BuyItemRequest model)
        {
            var result = await _itemSdkService.BuyItemAsync(model.PlayerId, model.ItemId);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        public class BuyItemRequest
        {
            public int PlayerId { get; set; }
            public int ItemId { get; set; }
        }
    }
}
