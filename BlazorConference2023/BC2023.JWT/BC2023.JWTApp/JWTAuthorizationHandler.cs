using BC2023.JWTApp.Logic;
using System.Net.Http.Headers;

namespace BC2023.JWTApp
{
    public class JWTAuthorizationHandler : DelegatingHandler
    {
        public StorageService _storageService { get; set; }

        public JWTAuthorizationHandler(StorageService storageService)
        {
            _storageService = storageService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var jwtToken = await _storageService.GetAccessTokensAsync();

            if (jwtToken != null)
                request.Headers.Authorization = new AuthenticationHeaderValue("bearer", jwtToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}