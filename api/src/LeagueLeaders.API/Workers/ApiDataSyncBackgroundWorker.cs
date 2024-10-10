using LeagueLeaders.Application.ApiDataSync;
using Microsoft.Extensions.Options;

namespace LeagueLeaders.API.Workers;

public class ApiDataSyncBackgroundWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly SyncSettings _syncSettings;

    public ApiDataSyncBackgroundWorker(
        IServiceScopeFactory scopeFactory,
        IOptions<SyncSettings> options)
    {
        _scopeFactory = scopeFactory;
        _syncSettings = options.Value;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await PerformSyncDataAsync();

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromHours(_syncSettings.FetchFrequencyHours), stoppingToken);

            await PerformSyncDataAsync();
        }
    }

    private async Task PerformSyncDataAsync()
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var _syncService = scope.ServiceProvider.GetRequiredService<IApiDataSyncService>();

            try
            {
                Console.WriteLine("-------------Syncing data from Spertradar API...");
                await _syncService.SyncDataAsync();
                await _syncService.ReportSuccessfulSyncronizationAsync("Sportradar API");
                Console.WriteLine("-------------Data synced successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"-------------Failed to sync data from Sportradar API. Reason: {ex.Message}");
                await _syncService.ReportFailedSyncronizationAsync("Sportradar API", ex.Message);
            }
        }
    }
}