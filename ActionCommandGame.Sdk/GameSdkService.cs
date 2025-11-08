using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ActionCommandGame.Dto;

namespace ActionCommandGame.Sdk
{
    public class GameSdkService
    {
        private readonly HttpClient _httpClient;

        public GameSdkService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GameResultDto?> PerformActionAsync(int playerId)
        {
            var payload = JsonSerializer.Serialize(new { playerId });
            var response = await _httpClient.PostAsync("/api/action/perform",
                new StringContent(payload, Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<GameResultDto>(content, JsonOptions());
        }

        private static JsonSerializerOptions JsonOptions() => new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }
}
