using MassTransit;
using PlaysFeed.Data;

public class MatchResultsConsumer : IConsumer<Batch<MatchResult>>
{
    private readonly IDeduplicationWriter _writer;
    private readonly ILogger<MatchResultsConsumer> _logger;

    public MatchResultsConsumer(IDeduplicationWriter writer, ILogger<MatchResultsConsumer> logger)
    {
        _writer = writer;
        _logger = logger;
    }

    public Task Consume(ConsumeContext<Batch<MatchResult>> context)
    {
        try
        {
            return _writer.WriteBatch(context.Message, context.CancellationToken);
        }
        catch(Exception e)
        {
            // Rethrow policy depends on requirements
            _logger.LogError(e, "Error while writing batch");
        }

        return Task.CompletedTask;
    }
}