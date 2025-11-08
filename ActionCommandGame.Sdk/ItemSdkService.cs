using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ActionCommandGame.Sdk
{
    public class ItemSdkService
    {
        private readonly HttpClient _httpClient;

        public ItemSdkService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ItemDto>?> GetItemsAsync()
        {
            var response = await _httpClient.GetAsync("/api/item");
            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<ItemDto>>(content, JsonOptions());
        }

        public async Task<BuyResultDto?> BuyItemAsync(int playerId, int itemId)
        {
            var payload = JsonSerializer.Serialize(new { playerId, itemId });
            var response = await _httpClient.PostAsync("/api/item/buy",
                new StringContent(payload, Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<BuyResultDto>(content, JsonOptions());
        }

        private static JsonSerializerOptions JsonOptions() => new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    // Placeholder DTOs
    public class ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
    }

    public class BuyResultDto
    {
        public PlayerDto Player { get; set; }
        public ItemDto Item { get; set; }
    }
}
