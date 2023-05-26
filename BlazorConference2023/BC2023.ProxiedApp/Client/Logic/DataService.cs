using System.Net.Http.Json;

namespace BC2023.ProxiedApp.Client.Logic
{
    public class DataService
    {
        private readonly HttpClient _http;

        public DataService(HttpClient http)
        {
            _http = http;
        }

        public Task<DateTime> GetServerCurrentDateTimeAsync()
        {
            return _http.GetFromJsonAsync<DateTime>("")!;
        }
    }
}