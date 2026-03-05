using System.Diagnostics;
using System.Text.Json;
using LuebeckRegatta.App.Models;
using LuebeckRegatta.App.Services;

namespace LuebeckRegatta.App.Repositories;

/// <summary>
/// Repository implementation for referee-related operations
/// </summary>
public class RefereeRepository : IRefereeRepository
{
    private readonly HttpClient _httpClient;
    private readonly ISettingsService _settingsService;

    public RefereeRepository(ISettingsService settingsService)
    {
        _httpClient = new HttpClient();
        _settingsService = settingsService;

        var username = _settingsService.RegattaUsername;
        var password = _settingsService.RegattaPassword;

        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            var credentials = Convert.ToBase64String(
                System.Text.Encoding.ASCII.GetBytes($"{username}:{password}"));
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
        }
    }

    public async Task<List<RefereeModel>?> GetRefereesAsync(int eventId, CancellationToken cancellationToken = default)
    {
        try
        {
            var baseUrl = _settingsService.RegattaApiUrl;
            var response = await _httpClient.GetStringAsync($"{baseUrl}/{eventId}/Referee/", cancellationToken);
            var referees = JsonSerializer.Deserialize<List<RefereeModel>>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return referees;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error fetching referees: {ex.Message}");
            return null;
        }
    }

    public async Task<string?> AddWarningAsync(int eventId, int refereeId, int heatId, CancellationToken cancellationToken = default)
    {
        try
        {
            var baseUrl = _settingsService.RegattaApiUrl;
            var response = await _httpClient.PutAsync($"{baseUrl}/{eventId}/Referee/{refereeId}/warning/{heatId}", null, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error adding warning: {ex.Message}");
            return null;
        }
    }
}
