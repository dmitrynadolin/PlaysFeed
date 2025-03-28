﻿using OpenQA.Selenium;
using PlaysFeed.Data;


namespace PlaysFeed.Futbolme;

/// <summary>
/// Represents a page with match results.
/// </summary>
public class ResultsPage
{
    private readonly IWebDriver _driver;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultsPage"/> class.
    /// </summary>
    /// <param name="driver">The web driver instance.</param>
    public ResultsPage(IWebDriver driver)
    {
        _driver = driver ?? throw new ArgumentNullException(nameof(driver));
    }

    /// <summary>
    /// Scrapes match results from the given URL.
    /// </summary>
    /// <param name="url">The URL to scrape.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>A list of match results.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the URL is null or empty.</exception>
    public async Task<IReadOnlyList<MatchResult>> GetMatchResultsAsync(string url, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentNullException(nameof(url));
        }

        _driver.Navigate().GoToUrl(url);
        Thread.Sleep(1000); // wait for JavaScript content to load

        var results = new List<MatchResult>();
        var games = _driver.FindElements(By.ClassName("cajaPartido")); // wrapper for all results

        foreach (var container in games)
        {
            var meta = container.FindElements(By.TagName("meta"));

            var result = new MatchResult
            {
                Sport = "Football",
                Competition = "LIGAT HA'AL"
            };

            foreach (var element in meta)
            {
                if (element.GetAttribute("itemprop").Contains("name"))
                {
                    // names of the teams
                    var teams = element.GetAttribute("content");

                    if (teams != null && teams.Contains(" - "))
                    {
                        var parts = teams.Split(" - ", 2, StringSplitOptions.TrimEntries);

                        if (parts.Length != 2)
                        {
                            continue;
                        }

                        result.LocalTeam = parts[0];
                        result.VisitingTeam = parts[1];
                    }

                    continue;
                }

                if (element.GetAttribute("itemprop").Contains("startDate"))
                {
                    var startDateString = element.GetAttribute("content");

                    if (DateTime.TryParse(startDateString, out var dateTime))
                    {
                        result.DateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
                    }
                }
            }

            var scores = container.FindElements(By.ClassName("resultadoPartido")); // scores

            foreach (var score in scores)
            {
                var scoreText = score.Text;

                if (scoreText.Contains(" - "))
                {
                    var parts = scoreText.Split(" - ", 2, StringSplitOptions.TrimEntries);
                    if (parts.Length != 2)
                    {
                        continue;
                    }
                    if (long.TryParse(parts[0], out var localScore))
                    {
                        result.LocalScore = localScore;
                    }
                    if (long.TryParse(parts[1], out var visitingScore))
                    {
                        result.VisitingScore = visitingScore;
                    }
                }
            }

            results.Add(result);
        }

        return results;
    }
}
