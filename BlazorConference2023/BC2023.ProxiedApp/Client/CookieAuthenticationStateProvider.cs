using BC2023.ProxiedApp.Client.Logic;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BC2023.ProxiedApp.Client
{
    public class CookieAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ProfileService _profileService;

        public CookieAuthenticationStateProvider(ProfileService profileService)
        {
            _profileService = profileService;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var currentUser = await _profileService.GetProfileAsync();

            if (currentUser != null)
            {
                var fnameClaim = new Claim("fname", currentUser.FirstName);
                var lnameClaim = new Claim("lname", currentUser.LastName);
                var fullNameClaim = new Claim("fullname", $"{currentUser.FirstName} {currentUser.LastName}");
                var subClaim = new Claim("sub", currentUser.Id.ToString());
                var usernameClaim = new Claim("username", currentUser.Username);

                var claimsIdentity = new ClaimsIdentity(new[] { subClaim, fullNameClaim, fnameClaim, lnameClaim, usernameClaim }, "serverAuth", "fullname", "role");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                return new AuthenticationState(claimsPrincipal);
            }
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }


        public async Task NotifyLoginAsync()
        {
            var currentUser = await _profileService.GetProfileAsync();
            var fnameClaim = new Claim("fname", currentUser.FirstName);
            var lnameClaim = new Claim("lname", currentUser.LastName);
            var fullNameClaim = new Claim("fullname", $"{currentUser.FirstName} {currentUser.LastName}");
            var subClaim = new Claim("sub", currentUser.Id.ToString());
            var usernameClaim = new Claim("username", currentUser.Username);

            var claimsIdentity = new ClaimsIdentity(new[] { subClaim, fullNameClaim, fnameClaim, lnameClaim, usernameClaim }, "serverAuth", "fullname", "role");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public void NotifyLogout()
        {
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
        }
    }
}
