namespace LeagueLeaders.Infrastructure.Clients.SportradarApi.Responses;

internal record CompetitionsResponse(CompetitionResponse Competition);

internal record CompetitionResponse(string Id, string Name, CategoryResponse Category);

internal record CategoryResponse(string Name);