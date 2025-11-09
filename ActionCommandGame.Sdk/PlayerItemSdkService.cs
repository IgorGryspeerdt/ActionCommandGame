using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Filters;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Sdk
{
    public class PlayerItemSdkService : IPlayerItemService
    {
        private readonly HttpClient _httpClient;

        public PlayerItemSdkService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServiceResult<PlayerItemResult>> Get(int id)
        {
            var response = await _httpClient.GetAsync($"/api/playeritem/{id}");
            if (!response.IsSuccessStatusCode)
                return new ServiceResult<PlayerItemResult>();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<PlayerItemResult>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return new ServiceResult<PlayerItemResult>(result!);
        }

        public async Task<ServiceResult<IList<PlayerItemResult>>> Find(PlayerItemFilter filter)
        {
            var response = await _httpClient.GetAsync($"/api/playeritem?playerId={filter.PlayerId}");
            if (!response.IsSuccessStatusCode)
                return new ServiceResult<IList<PlayerItemResult>>();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<PlayerItemResult>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return new ServiceResult<IList<PlayerItemResult>>(result ?? new List<PlayerItemResult>());
        }

        public async Task<ServiceResult<PlayerItemResult>> Create(int playerId, int itemId)
        {
            var payload = JsonSerializer.Serialize(new { playerId, itemId });
            var response = await _httpClient.PostAsync("/api/playeritem", new StringContent(payload, System.Text.Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode)
                return new ServiceResult<PlayerItemResult>();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<PlayerItemResult>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return new ServiceResult<PlayerItemResult>(result!);
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/playeritem/{id}");
            return new ServiceResult();
        }
    }
}
