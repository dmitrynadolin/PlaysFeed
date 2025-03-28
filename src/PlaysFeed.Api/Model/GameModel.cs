namespace PlaysFeed.Api.Model;

public class GameModel
{
    public string Sport { get; set; }
    public string Competition { get; set; }
    public string HomeTeam { get; set; }
    public string VisitingTeam { get; set; }
    public DateTime Date { get; set; }
}
