namespace LeagueLeaders.Infrastructure.Clients.SportradarApi.Responses;

internal record TeamStatsResponse();

internal record Statistics(Totals Totals);
internal record Totals(List<Competitor> Competitors);