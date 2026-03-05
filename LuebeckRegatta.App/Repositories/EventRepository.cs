using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using LuebeckRegatta.App.Models;
using LuebeckRegatta.App.Services;

namespace LuebeckRegatta.App.Repositories;

/// <summary>
/// Repository implementation for event-related operations
/// </summary>
public class EventRepository : IEventRepository
{
    private readonly HttpClient _httpClient;
    private readonly ISettingsService _settingsService;

    public EventRepository(ISettingsService settingsService)
    {
        _httpClient = new HttpClient();
        _settingsService = settingsService;
    }

    /// <summary>
    /// Configures Basic Authentication header for HTTP requests
    /// </summary>
    private void SetBasicAuthenticationHeader()
    {
        var username = _settingsService.RegattaUsername;
        var password = _settingsService.RegattaPassword;

        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        }
    }

    public async Task<List<EventModel>?> GetEventsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            SetBasicAuthenticationHeader();
            var baseUrl = _settingsService.RegattaApiUrl;
            var url = $"{baseUrl}/Event";
            var response = await _httpClient.GetStringAsync(url, cancellationToken);
            var events = JsonSerializer.Deserialize<List<EventModel>>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return events;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error fetching events: {ex.Message}");
            return null;
        }
    }

    public async Task<EventModel?> GetEventAsync(int eventId, CancellationToken cancellationToken = default)
    {
        try
        {
            SetBasicAuthenticationHeader();
            var baseUrl = _settingsService.RegattaApiUrl;
            var response = await _httpClient.GetStringAsync($"{baseUrl}/{eventId}/Event", cancellationToken);
            var eventModel = JsonSerializer.Deserialize<EventModel>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return eventModel;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error fetching event: {ex.Message}");
            return null;
        }
    }
}
