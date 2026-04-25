using de.openelp.regatta.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace de.openelp.regatta.Interfaces;

/// <summary>
/// Provides access to event endpoints.
/// </summary>
public interface IEventApiService
{
    /// <summary>
    /// Gets all available events.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A list of events, or <c>null</c> if loading failed.</returns>
    Task<List<EventModel>?> GetEventsAsync(CancellationToken ct = default);

    /// <summary>
    /// Gets a single event by its identifier.
    /// </summary>
    /// <param name="eventId">The event identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The event, or <c>null</c> if it was not found or loading failed.</returns>
    Task<EventModel?> GetEventAsync(int eventId, CancellationToken ct = default);

    /// <summary>
    /// Gets a single event by its name.
    /// </summary>
    /// <param name="eventName">The event name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The event, or <c>null</c> if it was not found or loading failed.</returns>
    Task<EventModel?> GetEventByNameAsync(string eventName, CancellationToken ct = default);
}
