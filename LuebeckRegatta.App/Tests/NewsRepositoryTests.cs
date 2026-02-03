using LuebeckRegatta.App.Repositories;
using Xunit;

namespace LuebeckRegatta.App.Tests;

public class NewsRepositoryTests
{
    [Fact]
    public async Task GetNewsFeedAsync_ReturnsValidFeed()
    {
        // Arrange
        var httpClient = new HttpClient();
        var repository = new NewsRepository(httpClient);

        // Act
        var result = await repository.GetNewsFeedAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Items);
        Assert.All(result.Items, item => 
        {
            Assert.NotNull(item.Title);
            Assert.NotNull(item.Link);
        });
    }

    [Fact]
    public void NewsItem_Properties_NotNull()
    {
        // Arrange & Act
        var newsItem = new Models.NewsItem
        {
            Title = "Test",
            Link = "https://test.com",
            Description = "Test Description"
        };

        // Assert
        Assert.Equal("Test", newsItem.Title);
        Assert.Equal("https://test.com", newsItem.Link);
        Assert.Equal("Test Description", newsItem.Description);
    }
}