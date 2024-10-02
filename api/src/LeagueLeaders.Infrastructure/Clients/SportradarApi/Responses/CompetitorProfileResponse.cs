namespace LeagueLeaders.Infrastructure.Clients.SportradarApi.Responses;

internal record CompetitorProfileResponse(CompetitorResponse Competitor, ManagerResponse Manager, VenueResponse Venue);

internal record CompetitorResponse(string Id, string Country, string Qualifier);

internal record ManagerResponse(string Name);

internal record VenueResponse(string Id, string Name, string CityName, string CountryName, int Capacity);