using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ActionCommandGame.Dto;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Results; // For IItemService
using ActionCommandGame.Services.Model;       // For ItemResult, ServiceResult


namespace ActionCommandGame.Sdk
{
    public class ItemSdkService : IItemService
    {
        private readonly HttpClient _httpClient;

        public ItemSdkService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServiceResult<ItemResult>> Get(int id)
        {
            var response = await _httpClient.GetAsync($"/api/item/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return new ServiceResult<ItemResult> { Data = null };
            }
            var content = await response.Content.ReadAsStringAsync();
            var itemDto = JsonSerializer.Deserialize<ItemDto>(content, JsonOptions());
            return new ServiceResult<ItemResult>
            {
                Data = itemDto != null ? MapToItemResult(itemDto) : null
            };
        }

        public async Task<ServiceResult<IList<ItemResult>>> Find()
        {
            var response = await _httpClient.GetAsync("/api/item");
            if (!response.IsSuccessStatusCode)
            {
                return new ServiceResult<IList<ItemResult>> { Data = null };
            }
            var content = await response.Content.ReadAsStringAsync();
            var itemDtos = JsonSerializer.Deserialize<List<ItemDto>>(content, JsonOptions());
            return new ServiceResult<IList<ItemResult>>
            {
                Data = itemDtos?.ConvertAll(MapToItemResult)
            };
        }

        public async Task<ServiceResult<ItemResult>> Buy(int playerId, int itemId)
        {
            var payload = JsonSerializer.Serialize(new { playerId, itemId });
            var response = await _httpClient.PostAsync("/api/item/buy",
                new StringContent(payload, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                return new ServiceResult<ItemResult> { Data = null };
            }

            var content = await response.Content.ReadAsStringAsync();
            var itemDto = JsonSerializer.Deserialize<ItemDto>(content, JsonOptions());
            return new ServiceResult<ItemResult>
            {
                Data = itemDto != null ? MapToItemResult(itemDto) : null
            };
        }

        private static ItemResult MapToItemResult(ItemDto dto)
        {
            // Map fields as appropriate
            return new ItemResult
            {
                Id = dto.Id,
                Name = dto.Name,
                Price = dto.Price,
                // Map other fields as needed
            };
        }

        private static JsonSerializerOptions JsonOptions() => new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }
}
