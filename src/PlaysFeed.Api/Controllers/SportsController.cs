using Microsoft.AspNetCore.Mvc;
using PlaysFeed.Api.Model;
using PlaysFeed.DataAccess;

namespace PlaysFeed.Api.Controllers;
/// <summary>
/// API Controller for managing sports.
/// </summary>
[ApiController]
[Route("[controller]")]
public class SportsController : ControllerBase
{
    private readonly GamesDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="SportsController"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public SportsController(GamesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Gets the list of sports.
    /// </summary>
    /// <returns>A list of sport models.</returns>
    [HttpGet]
    public IEnumerable<SportModel> Get()
    {
        return _dbContext.Sports.Select(x => new SportModel
        {
            Id = x.Id,
            Name = x.Name
        });
    }
}
