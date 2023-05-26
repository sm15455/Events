using System.Text.Json.Serialization;

namespace BC2023.JWTApp.Logic
{
    public class TokenResponse
    {
        [JsonPropertyName("accessToken")]
        public required string AccessToken { get; set; }
        [JsonPropertyName("refreshToken")]
        public required string RefreshToken { get; set; }
    }
}
