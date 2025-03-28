using Microsoft.AspNetCore.Mvc;
using PlaysFeed.Api.Model;
using PlaysFeed.DataAccess;

namespace PlaysFeed.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class TeamsController : ControllerBase
{
    private readonly GamesDbContext _dbContext;
    public TeamsController(GamesDbContext dbContext)
    {
        _dbContext = dbContext;
    }
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
