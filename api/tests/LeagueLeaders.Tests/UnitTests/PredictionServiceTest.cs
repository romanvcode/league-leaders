using FluentAssertions;
using LeagueLeaders.Application;
using LeagueLeaders.Application.Predictions;
using LeagueLeaders.Domain;
using LeagueLeaders.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace LeagueLeaders.Tests.UnitTests;

public class PredictionServiceTest : IDisposable
{
    private readonly LeagueLeadersDbContext _context;

    public PredictionServiceTest()
    {
        var options = new DbContextOptionsBuilder<LeagueLeadersDbContext>()
            .UseInMemoryDatabase(databaseName: "LeagueLeadersPredictionDB")
            .Options;

        _context = new LeagueLeadersDbContext(options);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
    }

    #region CreatePredictionAsync
    [Fact]
    public async Task CreatePredictionAsync_MatchNotFound_ShouldThrowException()
    {
        var _predictionService = new PredictionService(_context);

        var matchId = 1;
        var homeTeamScore = 1;
        var awayTeamScore = 1;


        var createPrediction = () => _predictionService.CreatePredictionAsync(matchId, homeTeamScore, awayTeamScore);


        await createPrediction.Should().ThrowAsync<TeamStatsNotFoundException>();
    }

    [Fact]
    public async Task CreatePredictionAsync_MatchStarted_ShouldThrowException()
    {
        var _predictionService = new PredictionService(_context);

        var match = new Match
        {
            Date = DateTime.Now.AddDays(-1)
        };

        await _context.Matches.AddAsync(match);
        await _context.SaveChangesAsync();

        var homeTeamScore = 1;
        var awayTeamScore = 1;


        var createPrediction = () => _predictionService.CreatePredictionAsync(match.Id, homeTeamScore, awayTeamScore);


        await createPrediction.Should().ThrowAsync<MatchAlreadyStartedException>();
    }

    [Fact]
    public async Task CreatePredictionAsync_PredictionExists_ShouldThrowException()
    {
        var _predictionService = new PredictionService(_context);

        var match = new Match
        {
            Date = DateTime.Now.AddDays(1)
        };

        await _context.Matches.AddAsync(match);

        var homeTeamScore = 1;
        var awayTeamScore = 1;

        var prediction = new Prediction
        {
            MatchId = match.Id,
            HomeTeamScore = homeTeamScore,
            AwayTeamScore = awayTeamScore,
        };

        await _context.Predictions.AddAsync(prediction);
        await _context.SaveChangesAsync();


        var createPrediction = () => _predictionService.CreatePredictionAsync(match.Id, homeTeamScore, awayTeamScore);


        await createPrediction.Should().ThrowAsync<PredictionAlreadyExistsException>();
    }

    [Fact]
    public async Task CreatePredictionAsync_ValidData_ShouldCreate()
    {
        var _predictionService = new PredictionService(_context);

        var match = new Match
        {
            Date = DateTime.Now.AddDays(1)
        };

        await _context.Matches.AddAsync(match);
        await _context.SaveChangesAsync();

        var homeTeamScore = 1;
        var awayTeamScore = 1;

        var expectedPrediction = new Prediction
        {
            Id = 1,
            HomeTeamScore = homeTeamScore,
            AwayTeamScore = awayTeamScore,
            MatchId = match.Id
        };


        var actualPrediction = await _predictionService.CreatePredictionAsync(match.Id, homeTeamScore, awayTeamScore);


        actualPrediction.Should().BeEquivalentTo(expectedPrediction);
    }
    #endregion

    #region GetPredictionsAsync
    [Fact]
    public async Task GetPredictionsAsync_NoPredictions_ShouldReturnEmptyList()
    {
        var _predictionService = new PredictionService(_context);


        var predictions = await _predictionService.GetPredictionsAsync();


        predictions.Should().BeEmpty();
    }

    [Fact]
    public async Task GetPredictionsAsync_ValidData_ShouldReturnPredictions()
    {
        var _predictionService = new PredictionService(_context);

        var match = new Match
        {
            Date = DateTime.Now.AddDays(1)
        };

        await _context.AddAsync(match);

        var homeTeamScore = 1;
        var awayTeamScore = 1;

        var prediction = new Prediction
        {
            Id = 1,
            HomeTeamScore = homeTeamScore,
            AwayTeamScore = awayTeamScore,
            MatchId = match.Id,
        };

        await _context.AddAsync(prediction);
        await _context.SaveChangesAsync();

        var expectedPredictions = new List<Prediction> 
        {
            prediction
        };


        var actualPredictions = await _predictionService.GetPredictionsAsync();


        actualPredictions.Should().BeEquivalentTo(expectedPredictions);
    }
    #endregion
}
