namespace PlaysFeed.DataAccess;

public class Team
{
    public int Id { get; set; }
    public string Name { get; set; }

    /// <summary>
    /// Sport Id
    /// </summary>
    public int SportId { get; set; }

    public virtual Sport Sport { get; set; }

    public virtual ICollection<Game> HomeGames { get; set; } = new HashSet<Game>();
    public virtual ICollection<Game> VisitingGames { get; set; } = new HashSet<Game>();
}