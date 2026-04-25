using de.openelp.regatta.Models;
using System.Threading;
using System.Threading.Tasks;

namespace de.openelp.regatta.Interfaces;

/// <summary>
/// Provides access to authentication endpoints.
/// </summary>
public interface IAuthApiService
{
    /// <summary>
    /// Authenticates a user.
    /// </summary>
    /// <param name="request">The login payload.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The authenticated user, or <c>null</c> if authentication failed.</returns>
    Task<UserDto?> LoginAsync(LoginRequestDto request, CancellationToken ct = default);
}
