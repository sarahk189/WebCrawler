using System.Text.RegularExpressions;
using WebCrawler.Core.Interfaces;
using WebCrawler.Core.Models;

namespace WebCrawler.Core.Services;

public class CrawlerService(HttpClient httpClient) : ICrawlerService
{
    private readonly HttpClient _httpClient = httpClient;

    //shows the pages we've checked so we don't check them again
    private readonly HashSet<string> _visitedUrls = new HashSet<string>();

    public async Task<CrawlResults> CrawlAsync(string rootUrl)
    {
        //check if the root URL is valid
        if (string.IsNullOrWhiteSpace(rootUrl))
        {
            throw new ArgumentException("Root URL cannot be null or empty.", nameof(rootUrl));
        }

        //convert the root URL to a Uri object
        var baseUri = new Uri(rootUrl);

        //start crawling! checking it out!
        await CrawlPageAsync(baseUri, baseUri);

        //return the results
        return new CrawlResults
        {
            Domain = baseUri.GetLeftPart(UriPartial.Authority), // just the scheme and host (e.g., https://example.com)
            Pages = _visitedUrls.OrderBy(x => x).ToList()           // sort for readability
        };
    }

    private IEnumerable<Uri> ExtractLinks(string html, Uri baseUri)
    {
        // reg expression to find href
        // will get <a href="https://example.com/contact">Contact</a>
        var hrefRegEx = new Regex("href=[\"'](?<url>.*?)[\"']", RegexOptions.IgnoreCase);
        var matches = hrefRegEx.Matches(html);

        foreach (Match match in matches)
        {
            var href = match.Groups["url"].Value;

            if (string.IsNullOrWhiteSpace(href)) continue;

            // skip links that aren’t navigational
            if (href.StartsWith("#") || href.StartsWith("mailto:") || href.StartsWith("javascript:"))
                continue;

            Uri resolvedUri;

            // try to create an absolute URI; if not, combine with baseUri to resolve relative links (adding /contact, /about, etc.)
            if (Uri.TryCreate(href, UriKind.Absolute, out resolvedUri) ||
                Uri.TryCreate(baseUri, href, out resolvedUri))
            {
                yield return resolvedUri;
            }
        }
    }

    //base uri is the root URL (e.g., https://example.com)
    //current uri is the URL of the page we're currently checking (e.g., https://example.com/contact)
    public async Task CrawlPageAsync(Uri baseUri, Uri currentUri)
    {

        var url = currentUri.ToString();

        //check if we've already visited this URL
        if (_visitedUrls.Contains(url) || currentUri.Host != baseUri.Host)
            return;


        //add the URL to the list of visited URLs
        _visitedUrls.Add(url);

        try
        {
            //get the HTML content of the page
            var html = await _httpClient.GetStringAsync(currentUri);

            foreach (var link in ExtractLinks(html, baseUri))
            {
                await CrawlPageAsync(baseUri, link);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error crawling {currentUri}: {ex.Message}");
        }
    }

}


