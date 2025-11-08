using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ActionCommandGame.Sdk
{
    public class AuthSdkService
    {
        private readonly HttpClient _httpClient;
        private string? _jwtToken;

        public AuthSdkService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public string? GetToken() => _jwtToken;

        public async Task<string?> RegisterAsync(string email, string password)
        {
            var payload = JsonSerializer.Serialize(new { email, password });
            var response = await _httpClient.PostAsync("/api/auth/register",
                new StringContent(payload, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<AuthResponse>(content, JsonOptions());
            _jwtToken = result?.Token;
            return _jwtToken;
        }

        public async Task<string?> LoginAsync(string email, string password)
        {
            var payload = JsonSerializer.Serialize(new { email, password });
            var response = await _httpClient.PostAsync("/api/auth/login",
                new StringContent(payload, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<AuthResponse>(content, JsonOptions());
            _jwtToken = result?.Token;
            return _jwtToken;
        }

        private static JsonSerializerOptions JsonOptions() => new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    // Placeholder DTO
    public class AuthResponse
    {
        public string Token { get; set; }
    }
}
