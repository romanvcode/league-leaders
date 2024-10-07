namespace LeagueLeaders.Application.ApiDataSync;

public interface IApiDataSyncService
{
    Task SyncDataAsync();
}