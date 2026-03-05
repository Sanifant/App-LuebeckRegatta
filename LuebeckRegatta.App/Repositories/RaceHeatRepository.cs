using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using LuebeckRegatta.App.Models;
using LuebeckRegatta.App.Services;

namespace LuebeckRegatta.App.Repositories;

/// <summary>
/// Repository implementation for race heat operations
/// </summary>
public class RaceHeatRepository : IRaceHeatRepository
{
    private readonly HttpClient _httpClient;
    private readonly ISettingsService _settingsService;

    public RaceHeatRepository(ISettingsService settingsService)
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

    public async Task<List<RaceHeatModel>?> GetRaceHeatsAsync(int eventId, CancellationToken cancellationToken = default)
    {
        try
        {
            SetBasicAuthenticationHeader();
            var baseUrl = _settingsService.RegattaApiUrl;
            var url = $"{baseUrl}/{eventId}/RaceHeat/";
            var response = await _httpClient.GetStringAsync(url, cancellationToken);
            var raceHeats = JsonSerializer.Deserialize<List<RaceHeatModel>>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return raceHeats;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error fetching race heats: {ex.Message}");
            return null;
        }
    }

    public async Task<RaceHeatModel?> GetRaceHeatAsync(int eventId, int raceId, CancellationToken cancellationToken = default)
    {
        try
        {
            SetBasicAuthenticationHeader();
            var baseUrl = _settingsService.RegattaApiUrl;
            var response = await _httpClient.GetStringAsync($"{baseUrl}/{eventId}/RaceHeat/{raceId}", cancellationToken);
            var raceHeat = JsonSerializer.Deserialize<RaceHeatModel>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return raceHeat;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error fetching race heat: {ex.Message}");
            return null;
        }
    }

    public async Task<string?> UpdateRaceHeatAsync(int eventId, RaceHeatModel raceHeat, CancellationToken cancellationToken = default)
    {
        try
        {
            SetBasicAuthenticationHeader();
            var baseUrl = _settingsService.RegattaApiUrl;
            var json = JsonSerializer.Serialize(raceHeat, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{baseUrl}/{eventId}/RaceHeat", content, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating race heat: {ex.Message}");
            return null;
        }
    }

    public async Task<string?> SetRefereeAsync(int eventId, int raceId, RefereeModel referee, CancellationToken cancellationToken = default)
    {
        try
        {
            SetBasicAuthenticationHeader();
            var baseUrl = _settingsService.RegattaApiUrl;
            var json = JsonSerializer.Serialize(referee, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{baseUrl}/{eventId}/RaceHeat/{raceId}/referee", content, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error setting referee: {ex.Message}");
            return null;
        }
    }

    public async Task<string?> StopRaceAsync(int eventId, int raceId, RaceHeatModel raceHeat, CancellationToken cancellationToken = default)
    {
        try
        {
            SetBasicAuthenticationHeader();
            var baseUrl = _settingsService.RegattaApiUrl;
            var json = JsonSerializer.Serialize(raceHeat, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{baseUrl}/{eventId}/RaceHeat/{raceId}/stop", content, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error stopping race: {ex.Message}");
            return null;
        }
    }
}
