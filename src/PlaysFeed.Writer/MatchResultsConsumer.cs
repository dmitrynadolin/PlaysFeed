using MassTransit;
using PlaysFeed.Data;

/// <summary>
/// Consumes a batch of match results and writes them to the storage.
/// </summary>
public class MatchResultsConsumer : IConsumer<Batch<MatchResult>>
{
    private readonly IDeduplicationWriter _writer;
    private readonly ILogger<MatchResultsConsumer> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="MatchResultsConsumer"/> class.
    /// </summary>
    /// <param name="writer">The deduplication writer.</param>
    /// <param name="logger">The logger instance.</param>
    public MatchResultsConsumer(IDeduplicationWriter writer, ILogger<MatchResultsConsumer> logger)
    {
        _writer = writer;
        _logger = logger;
    }

    /// <summary>
    /// Consumes a batch of match results and writes them to the storage.
    /// </summary>
    /// <param name="context">The consume context containing the batch of match results.</param>
    /// <returns>A task that represents the asynchronous consume operation.</returns>
    public Task Consume(ConsumeContext<Batch<MatchResult>> context)
    {
        try
        {
            return _writer.WriteBatch(context.Message, context.CancellationToken);
        }
        catch (Exception e)
        {
            // Rethrow policy depends on requirements
            _logger.LogError(e, "Error while writing batch");
        }

        return Task.CompletedTask;
    }
}
