using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Text.Json;

namespace BC2023.JWTApp.Logic
{
    public class AccountService
    {
        private readonly HttpClient _http;
        private readonly StorageService _storageService;
        private readonly JWTAuthenticationStateProvider _authenticationStateProvider;

        public AccountService(HttpClient http, StorageService storageService, AuthenticationStateProvider authenticationStateProvider)
        {
            _http = http;
            _storageService = storageService;
            _authenticationStateProvider = (JWTAuthenticationStateProvider)authenticationStateProvider;
        }

        public async Task<bool> AuthenticateAsync(string username, string password)
        {
            var result = await _http.PostAsJsonAsync("authenticate", new LoginRequest { Username = username, Password = password });
            var response = await result.Content.ReadAsStringAsync();
            if (String.IsNullOrWhiteSpace(response))
                return false;

            var tokens = JsonSerializer.Deserialize<TokenResponse>(response);
            await _storageService.SaveTokensAsync(tokens.AccessToken, tokens.RefreshToken);
            await _authenticationStateProvider.NotifyLoginAsync(tokens.AccessToken);
            return true;
        }

        public async Task LogoutAsync()
        {
            await _storageService.RemoveTokensAsync();
            _authenticationStateProvider.NotifyLogout();
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
    }
}