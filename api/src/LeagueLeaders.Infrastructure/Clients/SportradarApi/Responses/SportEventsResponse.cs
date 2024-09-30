namespace LeagueLeaders.Infrastructure.Clients.SportradarApi.Responses;

internal record SportEventsResponse(List<Summary> Summaries);

internal record Summary(SportEvent SportEvent);

internal record SportEvent(string Id, SportEventContext SportEventContext, List<Competitor> Competitors, string StartTime, Venue Venue);

internal record SportEventContext(Stage Stage);
internal record Stage(int Order);