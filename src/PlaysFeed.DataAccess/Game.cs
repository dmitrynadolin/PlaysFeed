namespace PlaysFeed.DataAccess;

public class Game
{
    public int Id { get; set; }
    /// <summary>
    /// Sport Id
    /// </summary>
    public int SportId { get; set; }
    public virtual Sport Sport { get; set; }
    public int CompetitionId { get; set; }
    public virtual Competition Competition { get; set; }
    public int HomeTeamId { get; set; }
    public virtual Team HomeTeam { get; set; }
    public int VisitingTeamId { get; set; }
    public virtual Team VisitingTeam { get; set; }
    public DateTime? Date { get; set; }
    public string Identity { get; set; }
}