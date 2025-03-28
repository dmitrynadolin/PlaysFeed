namespace PlaysFeed.Data;
public class MatchResult
{
    public string LocalTeam { get; set; }
    public string VisitingTeam { get; set; }
    public long LocalScore { get; set; }
    public long VisitingScore { get; set; }
    public DateTime? StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }

    public override string ToString()
    {
        return $"{StartDateTime:yyyy-MM-dd HH:mm} — {LocalTeam} vs {VisitingTeam} => {LocalScore}:{VisitingScore}";
    }
}