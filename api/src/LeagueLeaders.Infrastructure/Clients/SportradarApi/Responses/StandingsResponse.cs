namespace LeagueLeaders.Infrastructure.Clients.SportradarApi.Responses;

internal record StandingsResponse(List<StandingResponse> Standings);
internal record StandingResponse(List<GroupResponse> Groups);
internal record GroupResponse(StageResponse Stage, List<CompetitorWithStandingsResponse> Standings);
internal record CompetitorWithStandingsResponse(
    CompetitorResponse Competitor,
    int Points,
    int Rank,
    int Played,
    int Win,
    int Draw,
    int Loss,
    int GoalsFor,
    int GoalsAgainst
    );