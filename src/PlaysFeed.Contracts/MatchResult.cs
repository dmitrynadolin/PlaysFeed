namespace PlaysFeed.Data;
public class MatchResult
{
    public required string Competition { get; set; }
    public required string Sport { get; set; }
    public string LocalTeam { get; set; }
    public string VisitingTeam { get; set; }
    public DateTime? DateTime { get; set; }
    public long? LocalScore { get; set; }
    public long? VisitingScore { get; set; }

    public override string ToString()
    {
        return $"{DateTime:yyyy-MM-dd HH:mm} — {LocalTeam} vs {VisitingTeam} => {LocalScore}:{VisitingScore}";
    }
}