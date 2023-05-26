using BC2023.HostedApp.Client.Logic;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BC2023.HostedApp.Client
{
    public class CookieAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly AccountService _accountService;

        public CookieAuthenticationStateProvider(AccountService accountService)
        {
            _accountService = accountService;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var currentUser = await _accountService.GetProfileAsync();

            if (currentUser != null)
            {
                var fnameClaim = new Claim("fname", currentUser.FirstName);
                var lnameClaim = new Claim("lname", currentUser.LastName);
                var fullNameClaim = new Claim("fullname", $"{currentUser.FirstName} {currentUser.LastName}");
                var subClaim = new Claim("sub", currentUser.Id.ToString());
                var usernameClaim = new Claim("username", currentUser.Username);

                var claimsIdentity = new ClaimsIdentity(new[] { subClaim, fnameClaim, lnameClaim, fullNameClaim, usernameClaim }, "serverAuth", "fullname", "role");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                return new AuthenticationState(claimsPrincipal);
            }
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }
}
