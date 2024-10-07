namespace LeagueLeaders.Domain;

public class ApiDataSyncLog
{
    public int Id { get; set; }
    public string Source { get; set; }
    public DateTime SyncTime { get; set; }
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
}
