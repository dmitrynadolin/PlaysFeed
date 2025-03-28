using System;
using System.Text.Json;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using PlaysFeed.Contracts;
using PlaysFeed.Data;
using PlaysFeed.DataAccess;
using StackExchange.Redis;

/// <summary>
/// Writes deduplicated match results to the storage.
/// </summary>
public class DeduplicationWriter : IDeduplicationWriter
{
    private readonly GamesDbContext _dbContext;
    private readonly ILogger<DeduplicationWriter> _logger;
    private readonly IConnectionMultiplexer _redis;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeduplicationWriter"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="redis">The Redis connection multiplexer.</param>
    /// <param name="logger">The logger instance.</param>
    public DeduplicationWriter(GamesDbContext dbContext, IConnectionMultiplexer redis,
        ILogger<DeduplicationWriter> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
        _redis = redis;
    }

    /// <summary>
    /// Writes a batch of deduplicated match results to the storage.
    /// </summary>
    /// <param name="results">The batch of match results to write.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public async Task WriteBatch(Batch<MatchResult> results, CancellationToken cancellationToken)
    {
        var newGames = await SyncBatchWithDb(results, cancellationToken);

        // Write the new games to the Redis cache
        await AddGamesByCompetitionAsync(newGames);
    }

    /// <summary>
    /// Synchronizes the incoming batch of match results with the PostgreSQL database.
    /// </summary>
    /// <param name="results">The batch of match results.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of new games that were added to the database.</returns>
    private async Task<List<Game>> SyncBatchWithDb(Batch<MatchResult> results, CancellationToken cancellationToken)
    {
        var newGames = new List<Game>();

        var batchTeams = results
            .SelectMany(r => new[]
            {
                new { Team = r.Message.LocalTeam, r.Message.Sport },
                new { Team = r.Message.VisitingTeam, r.Message.Sport }
            }).ToArray();

        var teamSports = batchTeams.Select(n => n.Sport).Distinct().ToList();
        var teamNames = batchTeams.Select(n => n.Team).Distinct().ToList();

        var sports = await _dbContext.Sports.Where(s => teamSports.Contains(s.Name)).ToListAsync(cancellationToken);

        var sportIds = sports.Select(s => s.Id).ToList();

        // In real life, we are preloading only currently held competitions
        var competitions = await _dbContext.Competitions.ToListAsync(cancellationToken);

        // Preload only teams that are present in the batch
        var teams = await _dbContext.Teams
            .Where(t => sportIds.Contains(t.SportId))
            .Where(t => teamNames.Contains(t.Name))
            .ToListAsync(cancellationToken);

        foreach (var result in results)
        {
            var homeTeam = teams.FirstOrDefault(t => t.Name == result.Message.LocalTeam);
            var visitingTeam = teams.FirstOrDefault(t => t.Name == result.Message.VisitingTeam);
            var competition = competitions.FirstOrDefault(c => c.Name == result.Message.Competition);
            var sport = sports.FirstOrDefault(s => s.Name == result.Message.Sport);

            if (homeTeam == null
                || visitingTeam == null
                || competition == null
                || sport == null
                || result.Message.DateTime == null)
            {
                _logger.LogWarning("One or more entities not found for match result: {MatchResult}", result);
                continue;
            }

            var team1 = Math.Min(homeTeam.Id, visitingTeam.Id);
            var team2 = Math.Max(homeTeam.Id, visitingTeam.Id);

            var timeBucket = result.Message.DateTime.Value.ToFileTimeUtc() / 3600_000;

            // We can use some hashing to make the identity shorter
            var gameIdentityHash = $"{sport.Id}_{team1}_{team2}_{competition.Id}_{timeBucket}";
            //var gameIdentityHash = ComputeHash($"{sport.Id}_{team1}_{team2}_{competition.Id}_{timeBucket}");

            var existingGame =
                await _dbContext.Games.FirstOrDefaultAsync(g => g.Identity == gameIdentityHash, cancellationToken);

            if (existingGame != null) continue;

            var newGame = new Game
            {
                Sport = sport,
                Competition = competition,
                HomeTeamId = homeTeam.Id,
                HomeTeam = homeTeam,
                VisitingTeam = visitingTeam,
                Date = DateTime.SpecifyKind(result.Message.DateTime.Value, DateTimeKind.Utc),
                Identity = gameIdentityHash
            };

            _dbContext.Games.Add(newGame);

            newGames.Add(newGame);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return newGames;
    }

    /// <summary>
    /// Adds the new games to the Redis cache.
    /// </summary>
    /// <param name="games">The list of new games.</param>
    /// <returns>A task that represents the asynchronous add operation.</returns>
    private async Task AddGamesByCompetitionAsync(IEnumerable<Game> games)
    {
        var db = _redis.GetDatabase();

        foreach (var game in games)
        {
            var setKey = $"gameset:{game.Competition.Id}";
            var gameJson = JsonSerializer.Serialize(new CachedGame
            {
                Sport = game.Sport.Name,
                SportId = game.Sport.Id,
                HomeTeam = game.HomeTeam.Name,
                HomeTeamId = game.HomeTeam.Id,
                VisitingTeam = game.VisitingTeam.Name,
                VisitingTeamId = game.VisitingTeam.Id,
                CompetitionId = game.Competition.Id,
                Competition = game.Competition.Name,
                Date = game.Date.Value,
            });

            await db.SetAddAsync(setKey, gameJson);
            await db.KeyExpireAsync(setKey, TimeSpan.FromDays(3));
        }
    }
}
