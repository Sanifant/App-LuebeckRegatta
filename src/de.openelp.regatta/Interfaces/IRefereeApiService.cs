using de.openelp.regatta.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace de.openelp.regatta.Interfaces;

/// <summary>
/// Provides access to referee endpoints.
/// </summary>
public interface IRefereeApiService
{
    /// <summary>
    /// Gets all referees for the specified event.
    /// </summary>
    /// <param name="eventId">The event identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A list of referees, or <c>null</c> if loading failed.</returns>
    Task<List<RefereeModel>?> GetRefereesAsync(int eventId, CancellationToken ct = default);

    /// <summary>
    /// Adds a warning for a referee in a specific heat.
    /// </summary>
    /// <param name="eventId">The event identifier.</param>
    /// <param name="refereeId">The referee identifier.</param>
    /// <param name="heatId">The heat identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The backend response text, or <c>null</c> if the request failed.</returns>
    Task<string?> AddWarningAsync(int eventId, int refereeId, int heatId, CancellationToken ct = default);
}
