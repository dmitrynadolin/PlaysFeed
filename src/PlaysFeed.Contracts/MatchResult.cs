namespace PlaysFeed.Data;
public class MatchResult
{
    public required string Competition { get; set; }
    public required string Sport { get; set; }
    public string LocalTeam { get; set; }
    public string VisitingTeam { get; set; }
    public DateTime? DateTime { get; set; }

    /// <summary>
    /// Not used in the current version of the application.
    /// </summary>
    public long? LocalScore { get; set; }
    /// <summary>
    /// Not used in the current version of the application.
    /// </summary>
    public long? VisitingScore { get; set; }

    public override string ToString()
    {
        return $"{DateTime:yyyy-MM-dd HH:mm} — {LocalTeam} vs {VisitingTeam} => {LocalScore}:{VisitingScore}";
    }
}