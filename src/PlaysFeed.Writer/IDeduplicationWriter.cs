using MassTransit;
using PlaysFeed.Data;

/// <summary>
/// Writes deduplicated match results to the storage.
/// </summary>
public interface IDeduplicationWriter
{
    /// <summary>
    /// Writes a batch of deduplicated match results to the storage.
    /// </summary>
    /// <param name="results">The batch of match results to write.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    Task WriteBatch(Batch<MatchResult> results, CancellationToken cancellationToken);
}
