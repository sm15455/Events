namespace BC2023.JWTApp.Logic
{
    public class TokenUpdaterService
    {
        StorageService _storageService;
        public TokenUpdaterService(StorageService storageService)
        {
            _storageService = storageService;
        }
        public async Task StartExecuting()
        {
            using var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));
            while (await timer.WaitForNextTickAsync())
            {
            }
        }
    }
}
