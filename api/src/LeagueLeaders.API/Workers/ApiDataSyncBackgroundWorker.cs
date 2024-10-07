using LeagueLeaders.Application.ApiDataSync;
using Microsoft.Extensions.Options;

namespace LeagueLeaders.API.Workers;

public class ApiDataSyncBackgroundWorker : BackgroundService
{
    private readonly ILogger<ApiDataSyncBackgroundWorker> _logger;
    private readonly IApiDataSyncService _syncService;
    private readonly SyncSettings _syncSettings;

    public ApiDataSyncBackgroundWorker(
        ILogger<ApiDataSyncBackgroundWorker> logger,
        IApiDataSyncService service, 
        IOptions<SyncSettings> options)
    {
        _logger = logger;
        _syncService = service;
        _syncSettings = options.Value;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ApiDataSyncBackgroundWorker started.");

        await PerformSyncDataAsync();

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Waiting for next sync...");

            await Task.Delay(TimeSpan.FromHours(_syncSettings.FetchFrequencyHours), stoppingToken);

            await PerformSyncDataAsync();
        }
    }

    private async Task PerformSyncDataAsync()
    {
        _logger.LogInformation("Fetching data from API...");

        try
        {
            await _syncService.SyncDataAsync();
            _logger.LogInformation("API sync completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while syncing API data.");
        }
    }
}
