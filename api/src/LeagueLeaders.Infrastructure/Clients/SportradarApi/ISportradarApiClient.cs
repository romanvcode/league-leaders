using LeagueLeaders.Infrastructure.Clients.SportradarApi.Models;

namespace LeagueLeaders.Infrastructure.Clients.SportradarApi;
public interface ISportradarApiClient
{
    Task<Competition> GetCompetitionAsync();
    Task<List<Competitor>> GetCompetitorsAsync();
    Task<List<CompetitorStats>> GetCompetitorStatsAsync();
    Task<List<Player>> GetPlayersAsync();
    Task<List<PlayerStats>> GetPlayerStatsAsync();
    Task<List<Referee>> GetRefereesAsync();
    Task<List<Season>> GetSeasonsAsync();
    Task<List<SportEvent>> GetSportEventsAsync();
    Task<List<Stage>> GetStagesAsync();
    Task<List<Standing>> GetStandingsAsync();
    Task<List<Venue>> GetVenuesAsync();
}