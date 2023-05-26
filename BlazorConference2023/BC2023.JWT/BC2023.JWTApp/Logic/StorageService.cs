using Blazored.LocalStorage;

namespace BC2023.JWTApp.Logic
{
    public class StorageService
    {
        private readonly ILocalStorageService _localStorageService;
        private const string AccessTokenKey = "access_token";
        private const string RefreshTokenKey = "refresh_Token";

        public StorageService(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async ValueTask SaveTokensAsync(string accessToken, string refreshToken)
        {
            await _localStorageService.SetItemAsStringAsync(AccessTokenKey, accessToken);
            await _localStorageService.SetItemAsStringAsync(RefreshTokenKey, refreshToken);
        }

        public async ValueTask RemoveTokensAsync()
        {
            await _localStorageService.RemoveItemAsync(AccessTokenKey);
            await _localStorageService.RemoveItemAsync(RefreshTokenKey);
        }

        public ValueTask<string?> GetAccessTokensAsync()
        {
            return _localStorageService.GetItemAsStringAsync(AccessTokenKey);
        }

        public ValueTask<string?> GetRefreshTokensAsync()
        {
            return _localStorageService.GetItemAsStringAsync(RefreshTokenKey);
        }
    }
}