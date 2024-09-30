namespace LeagueLeaders.Infrastructure.Clients.SportradarApi.Responses;

internal record CompetitorProfileResponse(Competitor Competitor, Manager Manager, Venue Venue);

internal record Competitor(string Id, string Country, string Qualifier);

internal record Manager(string Name);

internal record Venue(string Id, string Name);