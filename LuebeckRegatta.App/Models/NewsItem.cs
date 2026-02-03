namespace LuebeckRegatta.App.Models;

public class NewsItem
{
    public string Title { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public DateTime PublishDate { get; set; }
    public string Author { get; set; } = string.Empty;
}