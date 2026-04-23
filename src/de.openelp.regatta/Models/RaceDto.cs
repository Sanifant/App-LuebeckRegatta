using System;
using System.Text.Json.Serialization;

namespace de.openelp.regatta.Models;

/// <summary>
/// Represents a race overview entry.
/// </summary>
public class RaceDto
{
    /// <summary>
    /// Gets or sets the race identifier.
    /// </summary>
    public int RaceId { get; set; } = 1175;

    /// <summary>
    /// Gets or sets the race number.
    /// </summary>
    [JsonPropertyName("raceNUmber")]
    public string? RaceNumber { get; set; }

    /// <summary>
    /// Gets or sets the race title.
    /// </summary>
    public string? RaceTitle { get; set; }

    /// <summary>
    /// Gets or sets the race start time.
    /// </summary>
    public DateTime? RaceStartTime { get; set; }
}
