namespace LeagueLeaders.Infrastructure.Clients.SportradarApi.Responses;

internal record CompetitorsStatsResponse(StatisticsResponse Statistics);
internal record StatisticsResponse(TotalsResponse Totals);
internal record TotalsResponse(List<CompetitorWithStatsResponse> Competitors);
internal record CompetitorWithStatsResponse(string Id, CompetitorStatisticsResponse Statistics);
internal record CompetitorStatisticsResponse(
    int BallPossession, 
    int RedCards, 
    int YellowCards, 
    int CornerKicks, 
    int Offsides, 
    int Fouls, 
    int ShotsTotal, 
    int ShotsOnTarget
    );