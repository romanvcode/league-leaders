namespace LeagueLeaders.Application.ApiDataSync;

public interface IApiDataSyncService
{
    Task LogApiDataSyncResultAsync(bool isSuccess, string? errorMessage = null);
    Task SyncDataAsync();
}