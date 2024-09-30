namespace LeagueLeaders.Infrastructure.Clients.SportradarApi.Responses;

internal record CompetitorProfileResponse(Competitor Competitor, Manager Manager, Venue Venue);

internal record Competitor(string Country);

internal record Manager(string Name);

internal record Venue(string Name);