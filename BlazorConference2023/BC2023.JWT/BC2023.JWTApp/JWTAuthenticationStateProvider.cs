using BC2023.JWTApp.Logic;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BC2023.JWTApp
{
    public class JWTAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ProfileService _profileService;
        private readonly StorageService _storageService;

        public JWTAuthenticationStateProvider(StorageService storageService, ProfileService profileService)
        {
            _profileService = profileService;
            _storageService = storageService;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var hasToken = (await _storageService.GetAccessTokensAsync()) is not null;
            if (hasToken)
            {
                var currentUser = await _profileService.GetProfileAsync();

                if (currentUser != null)
                {
                    var fnameClaim = new Claim("fname", currentUser.FirstName);
                    var lnameClaim = new Claim("lname", currentUser.LastName);
                    var fullNameClaim = new Claim("fullname", $"{currentUser.FirstName}{currentUser.LastName}");
                    var subClaim = new Claim("sub", currentUser.Id.ToString());
                    var usernameClaim = new Claim("username", currentUser.Username);

                    var claimsIdentity = new ClaimsIdentity(new[] { subClaim, fnameClaim, lnameClaim, fullNameClaim, usernameClaim }, "serverAuth", "fullname", "role");
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    return new AuthenticationState(claimsPrincipal);
                }
            }
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public async Task NotifyLoginAsync(string token)
        {
            var currentUser = await _profileService.GetProfileAsync();

            var fnameClaim = new Claim("fname", currentUser.FirstName);
            var lnameClaim = new Claim("lname", currentUser.LastName);
            var fullNameClaim = new Claim("fullname", $"{currentUser.FirstName}{currentUser.LastName}");
            var subClaim = new Claim("sub", currentUser.Id.ToString());
            var usernameClaim = new Claim("username", currentUser.Username);

            var claimsIdentity = new ClaimsIdentity(new[] { subClaim, fnameClaim, lnameClaim, fullNameClaim, usernameClaim }, "serverAuth", "fullname", "role");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        internal void NotifyLogout()
        {
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
        }
    }
}
