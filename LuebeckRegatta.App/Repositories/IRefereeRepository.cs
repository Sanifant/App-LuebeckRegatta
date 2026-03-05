using LuebeckRegatta.App.Models;

namespace LuebeckRegatta.App.Repositories;

/// <summary>
/// Repository interface for referee-related operations
/// </summary>
public interface IRefereeRepository
{
    /// <summary>
    /// Gets all referees for a specific event
    /// </summary>
    Task<List<RefereeModel>?> GetRefereesAsync(int eventId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a warning for a specific referee and heat
    /// </summary>
    Task<string?> AddWarningAsync(int eventId, int refereeId, int heatId, CancellationToken cancellationToken = default);
}
