﻿using LeagueLeaders.Infrastructure.Clients.SportradarApi.Models;

namespace LeagueLeaders.Infrastructure.Clients.SportradarApi;
public interface ISportradarApiClient
{
    Task<Competition> GetCompetitionAsync();
    Task<List<Season>> GetSeasonsAsync();
    Task<List<Stage>> GetStagesAsync();
    Task<List<SportEvent>> GetSportEventsAsync();
    Task<List<Competitor>> GetCompetitorsAsync();
    Task<List<Player>> GetPlayersAsync();
    Task<List<Referee>> GetRefereesAsync();
    Task<List<Venue>> GetVenuesAsync();
    Task<List<CompetitorStats>> GetCompetitorStatsAsync();
    Task<List<PlayerStats>> GetPlayerStatsAsync();
    Task<List<Standing>> GetStandingsAsync();
}