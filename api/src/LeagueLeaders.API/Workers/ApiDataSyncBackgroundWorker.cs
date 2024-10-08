using LeagueLeaders.Application.ApiDataSync;
using Microsoft.Extensions.Options;

namespace LeagueLeaders.API.Workers;

public class ApiDataSyncBackgroundWorker : BackgroundService
{
    private readonly IApiDataSyncService _syncService;
    private readonly SyncSettings _syncSettings;

    public ApiDataSyncBackgroundWorker(
        IApiDataSyncService service, 
        IOptions<SyncSettings> options)
    {
        _syncService = service;
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
        try
        {
            await _syncService.SyncDataAsync();
            await _syncService.LogApiDataSyncResultAsync(true);
        }
        catch (Exception ex)
        {
            await _syncService.LogApiDataSyncResultAsync(false, ex.Message);
        }
    }
}
