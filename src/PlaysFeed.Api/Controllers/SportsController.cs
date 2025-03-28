using Microsoft.AspNetCore.Mvc;

namespace PlaysFeed.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class SportsController : ControllerBase
{
    private static readonly string[] Sports = new[]
    {
        "Football", "Basketball", "Tennis", "Golf", "Baseball"
    };
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return Sports;
    }
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return Sports[id];
    }
}
