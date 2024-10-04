using LeagueLeaders.Domain;
using LeagueLeaders.Infrastructure.Clients.SportradarApi;
using LeagueLeaders.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace LeagueLeaders.Application;

public class ApiDataSyncService
{
    private readonly LeagueLeadersDbContext _context;
    private readonly ISportradarApiClient _sportradarApiClient;
    private readonly string _competitionPrefix = "sr:competition:";

    public ApiDataSyncService(LeagueLeadersDbContext context, ISportradarApiClient sportradarApiClient)
    {
        _context = context;
        _sportradarApiClient = sportradarApiClient;
    }

    public async Task SyncDataAsync()
    {
        var srCompetition = await _sportradarApiClient.GetCompetitionAsync();
        var dbCompetition = await _context.Competitions.FirstOrDefaultAsync
            (competiton => $"{_competitionPrefix}{competiton.SportradarId}" == srCompetition.Id);

        if (dbCompetition == null)
        {
            await _context.Competitions.AddAsync(new Competition
            {
                Name = srCompetition.Name,
                SportradarId = int.Parse(srCompetition.Id.ToLower().Replace(_competitionPrefix, ""))
            });
        }

        // ...

        await _context.SaveChangesAsync();
    }
}
