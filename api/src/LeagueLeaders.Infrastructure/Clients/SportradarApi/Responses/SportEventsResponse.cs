﻿namespace LeagueLeaders.Infrastructure.Clients.SportradarApi.Responses;

internal record SportEventsResponse(List<SummaryResponse> Summaries);

internal record SummaryResponse(SportEventResponse SportEvent, SportEventStatusResponse SportEventStatus);

internal record SportEventResponse(
    string Id, 
    SportEventContextResponse SportEventContext,
    SportEventConditionsResponse SportEventConditions,
    List<CompetitorResponse> Competitors, 
    string StartTime, 
    VenueResponse Venue
    );

internal record SportEventContextResponse(StageResponse Stage);
internal record StageResponse(int Order);

internal record SportEventConditionsResponse(List<RefereeResponse> Referees);
internal record RefereeResponse(string Id, string Name, string Nationality, string Type);

internal record SportEventStatusResponse(int HomeScore, int AwayScore);