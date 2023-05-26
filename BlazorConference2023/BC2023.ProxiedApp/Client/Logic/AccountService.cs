using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace BC2023.ProxiedApp.Client.Logic
{
    public class AccountService
    {
        private readonly HttpClient _http;
        private readonly CookieAuthenticationStateProvider _authenticationStateProvider;

        public AccountService(HttpClient http, AuthenticationStateProvider authenticationStateProvider)
        {
            _http = http;
            _authenticationStateProvider = (CookieAuthenticationStateProvider)authenticationStateProvider;
        }

        public async Task<bool> Authenticate(string username, string password)
        {
            var result = await _http.PostAsJsonAsync("account/login", new LoginRequest { Username = username, Password = password });
            var success = Convert.ToBoolean(await result.Content.ReadAsStringAsync());
            if (success)
            {
                await _authenticationStateProvider.NotifyLoginAsync();
            }
            return success;
        }

        public async Task LogoutAsync()
        {
            await _http.PostAsync("account/logout", new StringContent(String.Empty));
            _authenticationStateProvider.NotifyLogout();
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}