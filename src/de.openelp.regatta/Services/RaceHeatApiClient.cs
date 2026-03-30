using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using de.openelp.regatta.Models;

namespace de.openelp.regatta.Services;

public class RaceHeatApiClient
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _json;

    public RaceHeatApiClient(HttpClient http, JsonSerializerOptions? jsonOptions = null)
    {
        _http = http ?? throw new ArgumentNullException(nameof(http));
        _json = jsonOptions ?? new JsonSerializerOptions(JsonSerializerDefaults.Web);
    }

    /// <summary>
    /// GET /api/{eventId}/RaceHeat
    /// Liefert Heats (ohne Entries).
    /// </summary>
    public async Task<List<RaceHeatModel>> GetHeatsAsync(int eventId, CancellationToken ct = default)
    {
        var url = $"/api/{eventId}/RaceHeat";
        var result = await _http.GetFromJsonAsync<List<RaceHeatModel>>(url, _json, ct);
        return result ?? new List<RaceHeatModel>();
    }

    /// <summary>
    /// GET /api/{eventId}/RaceHeat/{raceId}
    /// Liefert ein Heat inkl. Entries.
    /// Achtung: Im Java-Code heißt der Pfadparameter {raceId}, tatsächlich wird aber nach Tables.RACE_HEAT.ID gefiltert.
    /// </summary>
    public async Task<RaceHeatModel?> GetHeatDetailsAsync(int eventId, int raceHeatId, CancellationToken ct = default)
    {
        var url = $"/api/{eventId}/RaceHeat/{raceHeatId}";
        return await _http.GetFromJsonAsync<RaceHeatModel>(url, _json, ct);
    }

    /// <summary>
    /// POST /api/{eventId}/RaceHeat/{raceId}/referee
    /// Setzt den Schiedsrichter für das Heat.
    /// </summary>
    public async Task SetRefereeAsync(int eventId, int raceHeatId, RefereeModel referee, CancellationToken ct = default)
    {
        var url = $"/api/{eventId}/RaceHeat/{raceHeatId}/referee";
        using var resp = await _http.PostAsJsonAsync(url, referee, _json, ct);
        resp.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// POST /api/{eventId}/RaceHeat/{raceId}/stop
    /// Stoppt das Rennen (setzt EndDate pro Entry).
    /// </summary>
    public async Task StopRaceAsync(int eventId, int raceHeatId, RaceHeatModel heat, CancellationToken ct = default)
    {
        var url = $"/api/{eventId}/RaceHeat/{raceHeatId}/stop";
        using var resp = await _http.PostAsJsonAsync(url, heat, _json, ct);
        resp.EnsureSuccessStatusCode();
    }
}