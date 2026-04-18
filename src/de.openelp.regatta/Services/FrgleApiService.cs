using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using de.openelp.regatta.Interfaces;
using de.openelp.regatta.Models;

namespace de.openelp.regatta.Services;

/// <summary>
/// Implementation of the frgle API service
/// </summary>
public class FrgleApiService : IFrgleApiService
{
    private readonly HttpClient _httpClient;
    private readonly IAppConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the FrgleApiService class
    /// </summary>
    /// <param name="httpClient">The HTTP client</param>
    /// <param name="configuration">Central app configuration</param>
    public FrgleApiService(IAppConfiguration? configuration = null)
    {
        _configuration = configuration ?? AppConfiguration.Current;
        
        var userName = String.IsNullOrEmpty(_configuration.UserName) ? throw new InvalidOperationException("Web API username must be configured.") : _configuration.UserName;
        var password = String.IsNullOrEmpty(_configuration.Password) ? throw new InvalidOperationException("Web API password must be configured.") : _configuration.Password;
        var apiBaseUrl = String.IsNullOrEmpty(_configuration.WebApiBaseUrl) ? throw new InvalidOperationException("Web API base URL must be configured.") : _configuration.WebApiBaseUrl;

        var byteArray = Encoding.ASCII.GetBytes($"{userName}:{password}");

        _httpClient = new HttpClient()
        {
            BaseAddress = new Uri(apiBaseUrl)
        };
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
    }

    /// <summary>
    /// Gets all referees for an event
    /// </summary>
    public async Task<List<RefereeModel>?> GetRefereesAsync(int eventId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_configuration.WebApiBaseUrl}/api/{eventId}/Referee");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<RefereeModel>>();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error getting referees: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Adds a warning to a referee for a specific heat
    /// </summary>
    public async Task<string?> AddWarningAsync(int refereeId, int heatId)
    {
        try
        {
            var response = await _httpClient.PutAsync(
                $"{_configuration.WebApiBaseUrl}/api/{_configuration.SelectedEventId}/Referee/{refereeId}/warning/{heatId}",
                null);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error adding warning: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Updates a race heat
    /// </summary>
    public async Task<string?> UpdateRaceAsync(RaceHeatModel raceHeat)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"{_configuration.WebApiBaseUrl}/api", raceHeat);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating race: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Gets all race heats for an event
    /// </summary>
    public async Task<List<RaceHeatModel>?> GetRaceHeatsAsync(int eventId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_configuration.WebApiBaseUrl}/api/{eventId}/RaceHeat");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<RaceHeatModel>>();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error getting race heats: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Gets a specific race heat
    /// </summary>
    public async Task<RaceHeatModel?> GetRaceHeatAsync(int eventId, int raceId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_configuration.WebApiBaseUrl}/api/{eventId}/RaceHeat/{raceId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<RaceHeatModel>();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error getting race heat: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Sets a referee for a race heat
    /// </summary>
    public async Task<string?> SetRefereeAsync(int raceId, RefereeModel referee)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                $"{_configuration.WebApiBaseUrl}/api/{_configuration.SelectedEventId}/RaceHeat/{raceId}/referee",
                referee);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error setting referee: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Stops a race
    /// </summary>
    public async Task<string?> StopRaceAsync(int eventId, int raceId, RaceHeatModel raceHeat)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                $"{_configuration.WebApiBaseUrl}/api/{eventId}/RaceHeat/{raceId}/stop",
                raceHeat);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error stopping race: {ex.Message}");
            return null;
        }
    }
}
