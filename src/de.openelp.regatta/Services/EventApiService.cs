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
/// Implements event endpoints from the frgle API.
/// </summary>
public class EventApiService : IEventApiService
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventApiService"/> class.
    /// </summary>
    /// <param name="configuration">Central app configuration.</param>
    public EventApiService(IAppConfiguration? configuration = null)
    {
        var appConfiguration = configuration ?? AppConfiguration.Current;
        var apiBaseUrl = string.IsNullOrWhiteSpace(appConfiguration.WebApiBaseUrl) ? throw new InvalidOperationException("Web API base URL must be configured.") : appConfiguration.WebApiBaseUrl;

        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(apiBaseUrl)
        };
    }

    /// <inheritdoc />
    public async Task<List<EventModel>?> GetEventsAsync(CancellationToken ct = default)
    {
        return await _httpClient.GetFromJsonAsync<List<EventModel>>("/api/Event", ct);
    }

    /// <inheritdoc />
    public async Task<EventModel?> GetEventAsync(int eventId, CancellationToken ct = default)
    {
        return await _httpClient.GetFromJsonAsync<EventModel>($"/api/{eventId}/Event", ct);
    }
}
