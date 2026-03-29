using System.Collections.Generic;
using System.Threading.Tasks;
using de.openelp.regatta.Models;

namespace de.openelp.regatta.Services;

/// <summary>
/// Interface for the frgle API service
/// </summary>
public interface IFrgleApiService
{
    /// <summary>
    /// Gets all referees for an event
    /// </summary>
    /// <param name="eventId">The event ID</param>
    /// <returns>List of referees</returns>
    Task<List<RefereeModel>?> GetRefereesAsync(int eventId);

    /// <summary>
    /// Adds a warning to a referee for a specific heat
    /// </summary>
    /// <param name="refereeId">The referee ID</param>
    /// <param name="heatId">The heat ID</param>
    /// <returns>Response message</returns>
    Task<string?> AddWarningAsync(int refereeId, int heatId);

    /// <summary>
    /// Updates a race heat
    /// </summary>
    /// <param name="raceHeat">The race heat to update</param>
    /// <returns>Response message</returns>
    Task<string?> UpdateRaceAsync(RaceHeatModel raceHeat);

    /// <summary>
    /// Gets all race heats for an event
    /// </summary>
    /// <param name="eventId">The event ID</param>
    /// <returns>List of race heats</returns>
    Task<List<RaceHeatModel>?> GetRaceHeatsAsync(int eventId);

    /// <summary>
    /// Gets a specific race heat
    /// </summary>
    /// <param name="eventId">The event ID</param>
    /// <param name="raceId">The race ID</param>
    /// <returns>The race heat</returns>
    Task<RaceHeatModel?> GetRaceHeatAsync(int eventId, int raceId);

    /// <summary>
    /// Sets a referee for a race heat
    /// </summary>
    /// <param name="raceId">The race ID</param>
    /// <param name="referee">The referee to assign</param>
    /// <returns>Response message</returns>
    Task<string?> SetRefereeAsync(int raceId, RefereeModel referee);

    /// <summary>
    /// Stops a race
    /// </summary>
    /// <param name="eventId">The event ID</param>
    /// <param name="raceId">The race ID</param>
    /// <param name="raceHeat">The race heat data</param>
    /// <returns>Response message</returns>
    Task<string?> StopRaceAsync(int eventId, int raceId, RaceHeatModel raceHeat);
}
