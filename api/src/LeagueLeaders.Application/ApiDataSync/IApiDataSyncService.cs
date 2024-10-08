namespace LeagueLeaders.Application.ApiDataSync;

public interface IApiDataSyncService
{
    Task ReportSuccessfulSyncronizationAsync();
    Task ReportFailedSyncronizationAsync(string reason);
    Task SyncDataAsync();
}