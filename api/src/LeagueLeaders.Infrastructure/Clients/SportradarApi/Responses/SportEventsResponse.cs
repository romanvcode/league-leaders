namespace LeagueLeaders.Infrastructure.Clients.SportradarApi.Responses;

internal record SportEventsResponse(List<Summary> Summaries);

internal record Summary(SportEvent SportEvent, SportEventStatus SportEventStatus);

internal record SportEvent(
    string Id, 
    SportEventContext SportEventContext,
    SportEventConditions SportEventConditions,
    List<Competitor> Competitors, 
    string StartTime, 
    Venue Venue
    );

internal record SportEventContext(Stage Stage);
internal record Stage(int Order);

internal record SportEventConditions(List<Models.Referee> Referees);

internal record SportEventStatus(int HomeScore, int AwayScore);