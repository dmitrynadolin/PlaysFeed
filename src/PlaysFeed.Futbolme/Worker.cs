using System.Collections;
using MassTransit;
using OpenQA.Selenium.Chrome;
using PlaysFeed.Data;

namespace PlaysFeed.Futbolme;

public class Worker : BackgroundService
{
    /// <summary>
    /// Poll interval. Should be read from configuration file.
    /// </summary>
    private const uint PollInterval = 15;

    private readonly ILogger<Worker> _logger;
    private readonly IPageRegistry _pageRegistry;
    private readonly IBus _publisher;
    private readonly IHostApplicationLifetime _lifetime;

    /// <summary>
    /// Initializes a new instance of the <see cref="Worker"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="pageRegistry">The page registry containing URLs to scrape.</param>
    /// <param name="publisher">The message bus publisher.</param>
    /// <param name="lifetime">The application lifetime manager.</param>
    public Worker(ILogger<Worker> logger, IPageRegistry pageRegistry, IBus publisher, IHostApplicationLifetime lifetime)
    {
        _logger = logger;
        _pageRegistry = pageRegistry;
        _publisher = publisher;
        _lifetime = lifetime;
    }

    /// <summary>
    /// Executes the background service.
    /// </summary>
    /// <param name="stoppingToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous execute operation.</returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless");

            using var driver = new ChromeDriver(options);

            while (!stoppingToken.IsCancellationRequested)
            {
                var delay = Task.Delay(TimeSpan.FromSeconds(PollInterval));

                await InnerLoopBody(driver, stoppingToken);

                await delay;
            }
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "Problems with the Chrome driver. Shutdown.");

            // Shutdown service so that support team can immediately see that it is not operational
            _lifetime.StopApplication();
        }
    }

    /// <summary>
    /// The inner loop body that performs the scraping and publishing.
    /// </summary>
    /// <param name="driver">The Chrome driver instance.</param>
    /// <param name="stoppingToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous inner loop operation.</returns>
    private async Task InnerLoopBody(ChromeDriver driver, CancellationToken stoppingToken)
    {
        try
        {
            var page = new ResultsPage(driver);

            foreach (var url in _pageRegistry.GetUrls())
            {
                _logger.LogDebug("Scraping {Url}", url);
                var matchResults =
                    await page.GetMatchResultsAsync(url, stoppingToken);

                // Publish results to MassTransit RabbitMQ
                await _publisher.PublishBatch(matchResults, stoppingToken);
            }
        }
        catch (Exception ex) // Inner loop never throws due to the transient errors
        {
            _logger.LogError(ex, "Failed to read data from the futbolme.");
        }
    }
}
