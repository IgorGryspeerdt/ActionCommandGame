using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ActionCommandGame.Dto;
using ActionCommandGame.Dto.requests;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Filters;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Sdk
{
    public class PlayerSdkService : IPlayerService
    {
        private readonly HttpClient _httpClient;

        public PlayerSdkService(HttpClient httpClient)
        {
            Console.WriteLine($"[DI DEBUG] PlayerSdkService constructed. BaseAddress: {httpClient.BaseAddress}");
            _httpClient = httpClient;
        }

        public async Task<ServiceResult<PlayerResult>> Get(int id, int userId)
        {
            var response = await _httpClient.GetAsync($"/api/player/{id}?userId={userId}");
            if (!response.IsSuccessStatusCode)
                return new ServiceResult<PlayerResult>();

            var content = await response.Content.ReadAsStringAsync();
            var player = JsonSerializer.Deserialize<PlayerResult>(content, JsonOptions());
            return new ServiceResult<PlayerResult>(player!);
        }

        public async Task<ServiceResult<IList<PlayerResult>>> Find(PlayerFilter? filter)
        {
            var query = "";
            if (filter != null)
            {
                var queryParams = new List<string>();
                if (filter.FilterUserPlayers.HasValue)
                    queryParams.Add($"filterUserPlayers={filter.FilterUserPlayers.Value.ToString().ToLower()}");
                if (filter.UserId.HasValue)
                    queryParams.Add($"userId={filter.UserId.Value}");
                if (queryParams.Count > 0)
                    query = "?" + string.Join("&", queryParams);
            }

            var response = await _httpClient.GetAsync($"/api/player{query}");
            if (!response.IsSuccessStatusCode)
                return new ServiceResult<IList<PlayerResult>>();

            var content = await response.Content.ReadAsStringAsync();
            var players = JsonSerializer.Deserialize<List<PlayerResult>>(content, JsonOptions());
            return new ServiceResult<IList<PlayerResult>>(players ?? new List<PlayerResult>());
        }

        public async Task<ServiceResult<PlayerResult>> Create(string name, int userId)
        {
            var payload = JsonSerializer.Serialize(new { name, userId });
            var response = await _httpClient.PostAsync("/api/player",
                new StringContent(payload, Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode)
                return new ServiceResult<PlayerResult>();

            var content = await response.Content.ReadAsStringAsync();
            var player = JsonSerializer.Deserialize<PlayerResult>(content, JsonOptions());
            return new ServiceResult<PlayerResult>(player!);
        }

     public async Task<ServiceResult<PlayerResult>> Update(UpdatePlayerRequest model, int userId)
        {
            var payload = JsonSerializer.Serialize(model);
            var response = await _httpClient.PutAsync($"/api/player/{model.Id}?userId={userId}",
                new StringContent(payload, Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode)
                return new ServiceResult<PlayerResult>();

            var content = await response.Content.ReadAsStringAsync();
            var player = JsonSerializer.Deserialize<PlayerResult>(content, JsonOptions());
            return new ServiceResult<PlayerResult>(player!);
        }

        public async Task<ServiceResult> Delete(int id, int userId)
        {
            var response = await _httpClient.DeleteAsync($"/api/player/{id}?userId={userId}");
            var result = new ServiceResult();
            // Optionally, you can add messages or set IsSuccess based on response
            return result;
        }

        private static JsonSerializerOptions JsonOptions() => new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }
}
