namespace LeagueLeaders.Infrastructure.Clients.SportradarApi.Responses;

internal record RefereesResponse(List<RefereeSummaryResponse> Summaries);
internal record RefereeSummaryResponse(RefereeSportEventResponse SportEvent);
internal record RefereeSportEventResponse(RefereeSportEventConditionsResponse SportEventConditions);
internal record RefereeSportEventConditionsResponse(List<RefereeResponse> Referees);
internal record RefereeResponse(string Id, string Name, string Nationality, string Type);