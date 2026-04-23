using de.openelp.regatta.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace de.openelp.regatta.Interfaces;

/// <summary>
/// Provides access to race heat endpoints.
/// </summary>
public interface IRaceHeatApiService
{
    /// <summary>
    /// Updates a race heat.
    /// </summary>
    /// <param name="raceHeat">The race heat payload.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The backend response text, or <c>null</c> if the request failed.</returns>
    Task<string?> UpdateRaceAsync(RaceHeatModel raceHeat, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to server-sent events.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Emitter metadata, or <c>null</c> if subscription failed.</returns>
    Task<SseEmitterModel?> SubscribeAsync(CancellationToken ct = default);

    /// <summary>
    /// Gets all race heats for the specified event.
    /// </summary>
    /// <param name="eventId">The event identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A list of race heats, or <c>null</c> if loading failed.</returns>
    Task<List<RaceHeatModel>?> GetRaceHeatsAsync(int eventId, CancellationToken ct = default);

    /// <summary>
    /// Gets a single race heat.
    /// </summary>
    /// <param name="eventId">The event identifier.</param>
    /// <param name="raceId">The race identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The race heat, or <c>null</c> if it was not found or loading failed.</returns>
    Task<RaceHeatModel?> GetRaceHeatAsync(int eventId, int raceId, CancellationToken ct = default);

    /// <summary>
    /// Assigns a referee to a race heat.
    /// </summary>
    /// <param name="eventId">The event identifier.</param>
    /// <param name="raceId">The race identifier.</param>
    /// <param name="referee">The referee payload.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The backend response text, or <c>null</c> if the request failed.</returns>
    Task<string?> SetRefereeAsync(int eventId, int raceId, RefereeModel referee, CancellationToken ct = default);

    /// <summary>
    /// Stops a race.
    /// </summary>
    /// <param name="eventId">The event identifier.</param>
    /// <param name="raceId">The race identifier.</param>
    /// <param name="raceHeat">The race heat payload.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The backend response text, or <c>null</c> if the request failed.</returns>
    Task<string?> StopRaceAsync(int eventId, int raceId, RaceHeatModel raceHeat, CancellationToken ct = default);
}
