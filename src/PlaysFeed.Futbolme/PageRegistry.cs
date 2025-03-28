namespace PlaysFeed.Futbolme;

/// <summary>
/// Default trivial implementation of the IPageRegistry interface.
/// </summary>
public class PageRegistry : IPageRegistry
{
    public IReadOnlyList<string> GetUrls()
    {
        return
        [
            "https://futbolme.com/resultados-directo/torneo/ligat-ha-al/162/"
        ];
    }
}