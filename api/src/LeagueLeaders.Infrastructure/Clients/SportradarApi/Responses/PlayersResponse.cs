namespace LeagueLeaders.Infrastructure.Clients.SportradarApi.Responses;

internal record PlayersResponse(List<SeasonCompetitorPlayersResponse> SeasonCompetitorPlayers);

internal record SeasonCompetitorPlayersResponse(string Id, List<Models.Player> Players);