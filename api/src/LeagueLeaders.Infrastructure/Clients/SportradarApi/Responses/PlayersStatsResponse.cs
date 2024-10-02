namespace LeagueLeaders.Infrastructure.Clients.SportradarApi.Responses;

internal record PlayersStatsResponse(PlayersStatisticsResponse Statistics);
internal record PlayersStatisticsResponse(PlayersTotalsResponse Totals);
internal record PlayersTotalsResponse(List<PlayersCompetitorWithStatsResponse> Competitors);
internal record PlayersCompetitorWithStatsResponse(List<PlayerWithStatsResponse> Players);
internal record PlayerWithStatsResponse(string Id, PlayerStatisticsResponse Statistics);
internal record PlayerStatisticsResponse(
    int GoalsScored,
    int Assists,
    int RedCards,
    int YellowCards,
    int ShotsOnTarget,
    int ShotsOffTarget
    );