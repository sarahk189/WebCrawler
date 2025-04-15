using WebCrawler.Core.Interfaces;
using WebCrawler.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// register crawler service through DI
builder.Services.AddHttpClient<ICrawlerService, CrawlerService>();

var app = builder.Build();