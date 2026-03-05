using LuebeckRegatta.App.Models;

namespace LuebeckRegatta.App.Repositories;

/// <summary>
/// Repository interface for race heat operations
/// </summary>
public interface IRaceHeatRepository
{
    /// <summary>
    /// Gets all race heats for a specific event
    /// </summary>
    Task<List<RaceHeatModel>?> GetRaceHeatsAsync(int eventId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a specific race heat by ID
    /// </summary>
    Task<RaceHeatModel?> GetRaceHeatAsync(int eventId, int raceId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a race heat
    /// </summary>
    Task<string?> UpdateRaceHeatAsync(int eventId, RaceHeatModel raceHeat, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets a referee for a specific race heat
    /// </summary>
    Task<string?> SetRefereeAsync(int eventId, int raceId, RefereeModel referee, CancellationToken cancellationToken = default);

    /// <summary>
    /// Stops a race
    /// </summary>
    Task<string?> StopRaceAsync(int eventId, int raceId, RaceHeatModel raceHeat, CancellationToken cancellationToken = default);
}