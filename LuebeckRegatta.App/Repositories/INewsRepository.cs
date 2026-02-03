using LuebeckRegatta.App.Models;

namespace LuebeckRegatta.App.Repositories;

public interface INewsRepository
{
    Task<NewsFeed?> GetNewsFeedAsync(CancellationToken cancellationToken = default);
}