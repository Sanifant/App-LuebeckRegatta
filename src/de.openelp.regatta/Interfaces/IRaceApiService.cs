using de.openelp.regatta.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace de.openelp.regatta.Interfaces;

/// <summary>
/// Provides access to race overview endpoints.
/// </summary>
public interface IRaceApiService
{
    /// <summary>
    /// Gets all races for the specified event.
    /// </summary>
    /// <param name="eventId">The event identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A list of races, or <c>null</c> if loading failed.</returns>
    Task<List<RaceDto>?> FindAllAsync(int eventId, CancellationToken ct = default);
}
