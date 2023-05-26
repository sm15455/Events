using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BC2023.HostedApp.Client.Logic
{
    public class AccountService
    {
        private readonly HttpClient _http;

        public AccountService(HttpClient http)
        {
            _http = http;
        }

        public async Task<bool> AuthenticateAsync(string username, string password)
        {
            var result = await _http.PostAsJsonAsync("authenticate", new LoginRequest { Username = username, Password = password });
            return Convert.ToBoolean(await result.Content.ReadAsStringAsync());
        }

        public async Task<UserModel?> GetProfileAsync()
        {
            var response = await _http.GetAsync("getprofile")!;
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<UserModel>(json);
            }
            return null;

        }

        public Task LogoutAsync()
        {
            return _http.PostAsync("logout", new StringContent(String.Empty));
        }
    }

    public class UserModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("username")]
        public string Username { get; set; } = String.Empty;
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; } = String.Empty;
        [JsonPropertyName("lastName")]
        public string LastName { get; set; } = String.Empty;
    }

    public class LoginRequest
    {
        public string Username { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
    }
}