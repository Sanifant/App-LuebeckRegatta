namespace LuebeckRegatta.App.Models;

public class NewsFeed
{
    public string Title { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public List<NewsItem> Items { get; set; } = new();
}