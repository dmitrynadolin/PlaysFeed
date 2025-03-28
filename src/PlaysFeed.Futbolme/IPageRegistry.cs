namespace PlaysFeed.Futbolme;


/// <summary>
/// Page Registry contains a list of URLs to scrape. If we need to start multiple scrapers, we can implement a more complex logic to distribute URLs between them.
/// </summary>
public interface IPageRegistry
{
    /// <summary>
    /// Get URLs to scrape from the Futbolme
    /// </summary>
    /// <returns></returns>
    IReadOnlyList<string> GetUrls();
}