namespace LeagueLeaders.Infrastructure.Clients.SportradarApi.Responses;

internal record CompetitionResponse(Competition Competition);

internal record Competition(string Id, string Name, Category Category);

internal record Category(string Name);