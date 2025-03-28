using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlaysFeed.Contracts;

/// <summary>
/// Represents a cached game in Redis.
/// </summary>
public class CachedGame
{
    public string Sport { get; set; }
    public int SportId { get; set; }
    public string Competition { get; set; }
    public int CompetitionId { get; set; }
    public string HomeTeam { get; set; }
    public int HomeTeamId { get; set; }
    public string VisitingTeam { get; set; }
    public int VisitingTeamId { get; set; }
    public DateTime Date { get; set; }
}
