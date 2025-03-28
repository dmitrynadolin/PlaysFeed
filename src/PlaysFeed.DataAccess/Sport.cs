namespace PlaysFeed.DataAccess;

/// <summary>
/// Represents a sport
/// </summary>
public class Sport
{
    /// <summary>
    /// Sport Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Sport name
    /// </summary>
    public string Name { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new HashSet<Game>();
    public virtual ICollection<Team> Teams { get; set; } = new HashSet<Team>();
    public virtual ICollection<Competition> Competitions { get; set; } = new HashSet<Competition>();
}