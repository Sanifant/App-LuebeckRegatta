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
/// Implements boat endpoints from the frgle API.
/// </summary>
public class BoatApiService : IBoatApiService
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="BoatApiService"/> class.
    /// </summary>
    /// <param name="configuration">Central app configuration.</param>
    public BoatApiService(IAppConfiguration? configuration = null)
    {
        var appConfiguration = configuration ?? AppConfiguration.Current;
        //var userName = string.IsNullOrWhiteSpace(appConfiguration.UserName) ? throw new InvalidOperationException("Web API username must be configured.") : appConfiguration.UserName;
        //var password = string.IsNullOrWhiteSpace(appConfiguration.Password) ? throw new InvalidOperationException("Web API password must be configured.") : appConfiguration.Password;
        var apiBaseUrl = string.IsNullOrWhiteSpace(appConfiguration.WebApiBaseUrl) ? throw new InvalidOperationException("Web API base URL must be configured.") : appConfiguration.WebApiBaseUrl;

        //var byteArray = Encoding.ASCII.GetBytes($"{userName}:{password}");

        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(apiBaseUrl)
        };
        //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
    }

    /// <inheritdoc />
    public async Task<List<BoatDto>?> GetAllBoatsAsync(int eventId, CancellationToken ct = default)
    {
        return await _httpClient.GetFromJsonAsync<List<BoatDto>>($"/api/{eventId}/boats", ct);
    }

    /// <inheritdoc />
    public async Task<List<BoatDto>?> GetAllBoatsPerRaceAsync(int eventId, int raceId, CancellationToken ct = default)
    {
        return await _httpClient.GetFromJsonAsync<List<BoatDto>>($"/api/{eventId}/boats/{raceId}", ct);
    }
}
