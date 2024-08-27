using LeagueLeaders.Domain;
using LeagueLeaders.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LeagueLeaders.Application
{
    public class TeamService : ITeamService
    {
        private readonly LeagueLeadersDbContext _context;

        public TeamService(LeagueLeadersDbContext context)
        {
            _context = context;
        }

        public async Task<Team?> GetTeamAsync(int teamId)
        {
            if (teamId == default)
            {
                throw new ArgumentNullException(nameof(teamId));
            }

            var team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == teamId);

            return team;
        }

        public async Task<List<Player>> GetTeamPlayersAsync(int teamId)
        {
            if (teamId == default)
            {
                throw new ArgumentNullException(nameof(teamId));
            }

            var players = await _context.Players.Where(p => p.TeamId == teamId).ToListAsync();

            return players;
        }

        public async Task<List<Match>> GetLastFiveTeamMatches(int teamId)
        {
            if (teamId == default)
            {
                throw new ArgumentNullException(nameof(teamId));
            }

            var currentStageId = await _context.Matches
                .Include(m => m.Stage)
                .GroupBy(m => m.StageId)
                .OrderByDescending(g => g.Key)
                .Select(g => g.Key)
                .FirstOrDefaultAsync();

            if (currentStageId == default)
            {
                return new List<Match>();
            }

            var matches = await _context.Matches
                .Where(m => m.HomeTeamId == teamId || m.AwayTeamId == teamId)
                .Where(m => m.StageId == currentStageId)
                .Where(m => m.Date < DateTime.UtcNow)
                .OrderByDescending(m => m.Date)
                .Take(5)
                .ToListAsync();

            return matches;
        }

        public async Task<List<Team>> GetTeamsBySearchTerm(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                throw new ArgumentNullException(nameof(searchTerm));
            }

            var teams = await _context.Teams
                .Where(t => t.Name.Contains(searchTerm))
                .ToListAsync();

            return teams;
        }
    }
}
