using WebCrawler.Core.Models;

namespace WebCrawler.Core.Interfaces;

public interface ICrawlerService
{
    Task<CrawlResults> CrawlAsync(string rootUrl);
}