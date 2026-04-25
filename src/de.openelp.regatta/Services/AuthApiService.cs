using de.openelp.regatta.Interfaces;
using de.openelp.regatta.Models;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace de.openelp.regatta.Services;

/// <summary>
/// Implements authentication endpoints from the frgle API.
/// </summary>
public class AuthApiService : IAuthApiService
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthApiService"/> class.
    /// </summary>
    /// <param name="configuration">Central app configuration.</param>
    public AuthApiService(IAppConfiguration? configuration = null)
    {
        var appConfiguration = configuration ?? AppConfiguration.Current;
        var apiBaseUrl = string.IsNullOrWhiteSpace(appConfiguration.WebApiBaseUrl)
            ? throw new InvalidOperationException("Web API base URL must be configured.")
            : appConfiguration.WebApiBaseUrl;

        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(apiBaseUrl)
        };
    }

    /// <inheritdoc />
    public async Task<UserDto?> LoginAsync(LoginRequestDto request, CancellationToken ct = default)
    {
        using var response = await _httpClient.PostAsJsonAsync("api/auth/login", request, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<UserDto>(ct);
    }
}
