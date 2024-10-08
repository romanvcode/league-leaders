namespace LeagueLeaders.Domain;

public class SyncLog
{
    public int Id { get; set; }
    public string Client { get; set; }
    public DateTime SyncTime { get; set; }
    public string Status { get; set; }
    public string Reason { get; set; }
}
