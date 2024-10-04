namespace LeagueLeaders.Infrastructure.Clients.SportradarApi.Responses;

internal record VenuesResponse(List<VenuesSummaryResponse> Summaries);
internal record VenuesSummaryResponse(VenuesSportEventResponse SportEvent);
internal record VenuesSportEventResponse(VenueResponse Venue);