using Microsoft.AspNetCore.Mvc;
using WebCrawler.Core.Interfaces;
using WebCrawler.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// register crawler service through DI
builder.Services.AddHttpClient<ICrawlerService, CrawlerService>();

var app = builder.Build();


app.MapGet("/pages", async (
    [FromServices] ICrawlerService crawlerService,
    [FromQuery] string target) =>
{
    if (string.IsNullOrWhiteSpace(target))
    {
        return Results.BadRequest(new { error = "Missing or invalid 'target' query parameter." });
    }

    try
    {
        var result = await crawlerService.CrawlAsync(target);
        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.Run();