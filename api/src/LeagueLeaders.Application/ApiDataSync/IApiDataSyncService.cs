namespace LeagueLeaders.Application.ApiDataSync;

public interface IApiDataSyncService
{
    Task ReportSuccessfulSyncronizationAsync(string client);
    Task ReportFailedSyncronizationAsync(string client, string reason);
    Task SyncDataAsync();
}