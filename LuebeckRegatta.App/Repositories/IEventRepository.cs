using LuebeckRegatta.App.Models;

namespace LuebeckRegatta.App.Repositories;

/// <summary>
/// Repository interface for event-related operations
/// </summary>
public interface IEventRepository
{
    /// <summary>
    /// Gets all events
    /// </summary>
    Task<List<EventModel>?> GetEventsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a specific event by ID
    /// </summary>
    Task<EventModel?> GetEventAsync(int eventId, CancellationToken cancellationToken = default);
}
