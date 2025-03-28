using MassTransit;
using PlaysFeed.Data;

/// <summary>
/// Writes deduplicated match results to the storage.
/// </summary>
public interface IDeduplicationWriter
{
    Task WriteBatch(Batch<MatchResult> results, CancellationToken cancellationToken);
}