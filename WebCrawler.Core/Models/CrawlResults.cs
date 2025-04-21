
namespace WebCrawler.Core.Models;

public class CrawlResults
{

    public string Domain { get; set; } = string.Empty;
    public List<string> Pages { get; set; } = new List<string>();
}
//{
//  "domain": "https://example.com",
//  "pages": [
//    "https://example.com/",
//    "https://example.com/contact.html"
//  ]
//}

