namespace LeagueLeaders.Infrastructure.Clients.SportradarApi.Responses;

internal record SchedulesResponse(List<ScheduleResponse> Schedules);
internal record ScheduleResponse(ScheduleSEResponse SportEvent, ScheduleSEStatusResponse SportEventStatus);

internal record ScheduleSEResponse(
    string Id,
    ScheduleSEContextResponse SportEventContext,
    List<CompetitorResponse> Competitors,
    string StartTime,
    VenueResponse Venue
    );

internal record ScheduleSEContextResponse(ScheduleStageResponse Stage);
internal record ScheduleStageResponse(int Order);

internal record ScheduleSEStatusResponse(string Status, int? HomeScore, int? AwayScore);