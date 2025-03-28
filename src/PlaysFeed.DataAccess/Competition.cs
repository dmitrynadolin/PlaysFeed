namespace PlaysFeed.DataAccess;

/// <summary>
/// Represents a competition
/// </summary>
public class Competition
{
    /// <summary>
    /// Competition Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Competition name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Sport Id
    /// </summary>
    public int SportId { get; set; }

    public virtual Sport Sport { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new HashSet<Game>();
}