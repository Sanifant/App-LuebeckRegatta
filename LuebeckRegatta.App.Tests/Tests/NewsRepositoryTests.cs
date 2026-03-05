using LuebeckRegatta.App.Repositories;
using LuebeckRegatta.App.Services;
using LuebeckRegatta.App.Models;
using Xunit;

namespace LuebeckRegatta.App.Tests;

/// <summary>
/// Unit tests für NewsRepository
/// </summary>
public class NewsRepositoryTests
{
    /// <summary>
    /// Testet, dass NewsRepository ohne Fehler initialisiert werden kann
    /// </summary>
    [Fact]
    public void NewsRepository_CanBeInstantiated()
    {
        // Arrange
        var settingsService = new SettingsService();
        
        // Act
        var repository = new NewsRepository(settingsService);

        // Assert
        Assert.NotNull(repository);
    }

    /// <summary>
    /// Testet, dass NewsItem Properties korrekt gesetzt werden können
    /// </summary>
    [Fact]
    public void NewsItem_Properties_CanBeSet()
    {
        // Arrange
        var testDate = DateTime.Now;
        
        // Act
        var newsItem = new NewsItem
        {
            Title = "Test Titel",
            Link = "https://test.com",
            Description = "Test Beschreibung",
            PublishDate = testDate,
            ImageUrl = "https://test.com/image.jpg"
        };

        // Assert
        Assert.Equal("Test Titel", newsItem.Title);
        Assert.Equal("https://test.com", newsItem.Link);
        Assert.Equal("Test Beschreibung", newsItem.Description);
        Assert.Equal(testDate, newsItem.PublishDate);
        Assert.Equal("https://test.com/image.jpg", newsItem.ImageUrl);
    }

    /// <summary>
    /// Testet, dass NewsFeed Properties korrekt gesetzt werden können
    /// </summary>
    [Fact]
    public void NewsFeed_Properties_CanBeSet()
    {
        // Arrange & Act
        var feed = new NewsFeed
        {
            Title = "Test Feed",
            Link = "https://test.com",
            Language = "de",
            Items = new List<NewsItem>()
        };

        // Assert
        Assert.Equal("Test Feed", feed.Title);
        Assert.Equal("https://test.com", feed.Link);
        Assert.Equal("de", feed.Language);
        Assert.NotNull(feed.Items);
        Assert.Empty(feed.Items);
    }
}