using System.Net.Http.Json;

namespace BC2023.JWTApp.Logic
{
    public class ProfileService
    {
        private readonly HttpClient _http;

        public ProfileService(HttpClient http)
        {
            _http = http;
        }

        public Task<UserModel> GetProfileAsync()
        {
            return _http.GetFromJsonAsync<UserModel>("getprofile")!;
        }
    }

    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; } = String.Empty;
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
    }
}