using de.openelp.regatta.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace de.openelp.regatta.Interfaces;

/// <summary>
/// Provides access to boat endpoints.
/// </summary>
public interface IBoatApiService
{
    /// <summary>
    /// Gets all boats for the specified event.
    /// </summary>
    /// <param name="eventId">The event identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A list of boats, or <c>null</c> if loading failed.</returns>
    Task<List<BoatDto>?> GetAllBoatsAsync(int eventId, CancellationToken ct = default);

    /// <summary>
    /// Gets all boats for a specific race in an event.
    /// </summary>
    /// <param name="eventId">The event identifier.</param>
    /// <param name="raceId">The race identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A list of boats, or <c>null</c> if loading failed.</returns>
    Task<List<BoatDto>?> GetAllBoatsPerRaceAsync(int eventId, int raceId, CancellationToken ct = default);
}
