using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlaysFeed.Api.Model;
using PlaysFeed.Contracts;
using PlaysFeed.DataAccess;
using StackExchange.Redis;

namespace PlaysFeed.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class CompetitionsController : ControllerBase
{
    private readonly GamesDbContext _dbContext;
    private readonly IConnectionMultiplexer _redis;

    public CompetitionsController(GamesDbContext dbContext, IConnectionMultiplexer redis)
    {
        _dbContext = dbContext;
        _redis = redis;
    }
    [HttpGet]
    public async Task<IEnumerable<CompetitionModel>> Get(CancellationToken cancellationToken)
    {
        return await _dbContext.Competitions.Select(x => new CompetitionModel
        {
            Id = x.Id,
            Name = x.Name
        }).ToListAsync(cancellationToken);
    }

    [HttpGet("{id}/games")]
    public async Task<IEnumerable<GameModel>> Get(int id)
    {
        var results = new List<GameModel>();

        // Read from Redis
        var db = _redis.GetDatabase();

        var key = $"gameset:{id}";

        var members = await db.SetMembersAsync(key);
        foreach (var member in members)
        {
            var jsonValue = member.ToString();

            var game = JsonSerializer.Deserialize<CachedGame>(jsonValue);

            results.Add(new GameModel
            {
                Sport = game.Sport,
                Competition = game.Competition,
                HomeTeam = game.HomeTeam,
                VisitingTeam = game.VisitingTeam,
                Date = game.Date
            });
        }

        return results;
    }
}
