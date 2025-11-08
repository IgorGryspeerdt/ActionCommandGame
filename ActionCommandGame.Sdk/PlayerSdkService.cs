using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ActionCommandGame.Sdk
{
    public class PlayerSdkService
    {
        private readonly HttpClient _httpClient;

        public PlayerSdkService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PlayerDto>?> GetPlayersAsync()
        {
            var response = await _httpClient.GetAsync("/api/player");
            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<PlayerDto>>(content, JsonOptions());
        }

        public async Task<PlayerDto?> GetPlayerAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/player/{id}");
            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PlayerDto>(content, JsonOptions());
        }

        public async Task<PlayerDto?> CreatePlayerAsync(string name)
        {
            var payload = JsonSerializer.Serialize(new { name });
            var response = await _httpClient.PostAsync("/api/player",
                new StringContent(payload, Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PlayerDto>(content, JsonOptions());
        }

        public async Task<bool> DeletePlayerAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/player/{id}");
            return response.IsSuccessStatusCode;
        }

        private static JsonSerializerOptions JsonOptions() => new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    // Placeholder DTO
    public class PlayerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Money { get; set; }
        public int Experience { get; set; }
    }
}
