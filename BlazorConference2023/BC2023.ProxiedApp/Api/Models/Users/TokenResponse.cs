﻿namespace BC2023.ProxiedApp.Api.Models.Users
{
    public class TokenResponse
    {
        public required string AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
