using System.Xml.Linq;
using LuebeckRegatta.App.Models;

namespace LuebeckRegatta.App.Repositories;

public class NewsRepository : INewsRepository
{
    private readonly HttpClient _httpClient;
    private const string FeedUrl = "https://www.rudern.de/news.xml";

    public NewsRepository()
    {
        _httpClient = new HttpClient();
    }

    public async Task<NewsFeed?> GetNewsFeedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetStringAsync(FeedUrl, cancellationToken);
            var doc = XDocument.Parse(response);
            var channel = doc.Root?.Element("channel");

            if (channel == null)
                return null;

            var feed = new NewsFeed
            {
                Title = channel.Element("title")?.Value ?? string.Empty,
                Link = channel.Element("link")?.Value ?? string.Empty,
                Language = channel.Element("language")?.Value ?? string.Empty,
                Items = channel.Elements("item").Select(ParseNewsItem).ToList()
            };

            return feed;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private static NewsItem ParseNewsItem(XElement item)
    {
        var description = item.Element("description")?.Value ?? string.Empty;
        var imageUrl = ExtractImageUrl(description);

        return new NewsItem
        {
            Title = item.Element("title")?.Value ?? string.Empty,
            Link = item.Element("link")?.Value ?? string.Empty,
            Description = StripHtml(description),
            ImageUrl = imageUrl,
            PublishDate = ParseDate(item.Element("pubDate")?.Value),
            Author = item.Element("author")?.Value ?? string.Empty
        };
    }

    private static string ExtractImageUrl(string html)
    {
        var srcMatch = System.Text.RegularExpressions.Regex.Match(html, @"src=""([^""]+)""");
        return srcMatch.Success ? srcMatch.Groups[1].Value : string.Empty;
    }

    private static string StripHtml(string html)
    {
        return System.Text.RegularExpressions.Regex.Replace(html, "<.*?>", string.Empty).Trim();
    }

    private static DateTime ParseDate(string? dateString)
    {
        if (string.IsNullOrEmpty(dateString))
            return DateTime.MinValue;

        return DateTime.TryParse(dateString, out var date) ? date : DateTime.MinValue;
    }
}