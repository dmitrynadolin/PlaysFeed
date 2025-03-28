using Microsoft.AspNetCore.Mvc;
using PlaysFeed.Api.Model;
using PlaysFeed.DataAccess;

namespace PlaysFeed.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class TeamsController : ControllerBase
{
    private readonly GamesDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="TeamsController"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public TeamsController(GamesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Gets the list of teams.
    /// </summary>
    /// <returns>A list of team models.</returns>
    [HttpGet]
    public IEnumerable<TeamModel> Get()
    {
        return _dbContext.Teams.Select(x => new TeamModel
        {
            Id = x.Id,
            Name = x.Name
        });
    }
}
