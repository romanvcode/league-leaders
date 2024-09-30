namespace LeagueLeaders.Infrastructure.Clients.SportradarApi.Responses;

internal record PlayersResponse(List<SeasonCompetitorPlayers> SeasonCompetitorPlayers);

internal record SeasonCompetitorPlayers(string Id, List<Models.Player> Players);