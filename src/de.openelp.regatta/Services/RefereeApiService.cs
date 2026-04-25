using de.openelp.regatta.Interfaces;
using de.openelp.regatta.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace de.openelp.regatta.Services;

/// <summary>
/// Implements referee endpoints from the frgle API.
/// </summary>
public class RefereeApiService : IRefereeApiService
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="RefereeApiService"/> class.
    /// </summary>
    /// <param name="configuration">Central app configuration.</param>
    public RefereeApiService(IAppConfiguration? configuration = null)
    {
        var appConfiguration = configuration ?? AppConfiguration.Current;
        var userName = string.IsNullOrWhiteSpace(appConfiguration.UserName) ? throw new InvalidOperationException("Web API username must be configured.") : appConfiguration.UserName;
        var password = string.IsNullOrWhiteSpace(appConfiguration.Password) ? throw new InvalidOperationException("Web API password must be configured.") : appConfiguration.Password;
        var apiBaseUrl = string.IsNullOrWhiteSpace(appConfiguration.WebApiBaseUrl) ? throw new InvalidOperationException("Web API base URL must be configured.") : appConfiguration.WebApiBaseUrl;

        var byteArray = Encoding.ASCII.GetBytes($"{userName}:{password}");

        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(apiBaseUrl)
        };
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
    }

    /// <inheritdoc />
    public async Task<List<RefereeModel>?> GetRefereesAsync(int eventId, CancellationToken ct = default)
    {
        return await _httpClient.GetFromJsonAsync<List<RefereeModel>>($"/api/{eventId}/Referee", ct);
    }

    /// <inheritdoc />
    public async Task<string?> AddWarningAsync(int eventId, int refereeId, int heatId, CancellationToken ct = default)
    {
        using var response = await _httpClient.PutAsync($"/api/{eventId}/Referee/{refereeId}/warning/{heatId}", null, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(ct);
    }
}
