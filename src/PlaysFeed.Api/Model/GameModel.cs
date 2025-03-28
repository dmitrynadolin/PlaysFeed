namespace PlaysFeed.Api.Model;

public class GameModel
{
    /// <summary>
    /// Gets or sets the sport name.
    /// </summary>
    public string Sport { get; set; }

    /// <summary>
    /// Gets or sets the competition name.
    /// </summary>
    public string Competition { get; set; }

    /// <summary>
    /// Gets or sets the home team name.
    /// </summary>
    public string HomeTeam { get; set; }

    /// <summary>
    /// Gets or sets the visiting team name.
    /// </summary>
    public string VisitingTeam { get; set; }

    /// <summary>
    /// Gets or sets the date of the game.
    /// </summary>
    public DateTime Date { get; set; }
}
