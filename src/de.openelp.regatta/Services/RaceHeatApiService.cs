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
/// Implements race heat endpoints from the frgle API.
/// </summary>
public class RaceHeatApiService : IRaceHeatApiService
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="RaceHeatApiService"/> class.
    /// </summary>
    /// <param name="configuration">Central app configuration.</param>
    public RaceHeatApiService(IAppConfiguration? configuration = null)
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
    public async Task<string?> UpdateRaceAsync(RaceHeatModel raceHeat, CancellationToken ct = default)
    {
        using var response = await _httpClient.PutAsJsonAsync("/api", raceHeat, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(ct);
    }

    /// <inheritdoc />
    public async Task<SseEmitterModel?> SubscribeAsync(CancellationToken ct = default)
    {
        using var response = await _httpClient.GetAsync("/api/subscribe", ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<SseEmitterModel>(ct);
    }

    /// <inheritdoc />
    public async Task<List<RaceHeatModel>?> GetRaceHeatsAsync(int eventId, CancellationToken ct = default)
    {
        return await _httpClient.GetFromJsonAsync<List<RaceHeatModel>>($"/api/{eventId}/RaceHeat", ct);
    }

    /// <inheritdoc />
    public async Task<RaceHeatModel?> GetRaceHeatAsync(int eventId, int raceId, CancellationToken ct = default)
    {
        return await _httpClient.GetFromJsonAsync<RaceHeatModel>($"/api/{eventId}/RaceHeat/{raceId}", ct);
    }

    /// <inheritdoc />
    public async Task<string?> SetRefereeAsync(int eventId, int raceId, RefereeModel referee, CancellationToken ct = default)
    {
        using var response = await _httpClient.PostAsJsonAsync($"/api/{eventId}/RaceHeat/{raceId}/referee", referee, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(ct);
    }

    /// <inheritdoc />
    public async Task<string?> StopRaceAsync(int eventId, int raceId, RaceHeatModel raceHeat, CancellationToken ct = default)
    {
        using var response = await _httpClient.PostAsJsonAsync($"/api/{eventId}/RaceHeat/{raceId}/stop", raceHeat, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(ct);
    }
}
