using System;
using System.Text.Json.Serialization;

namespace de.openelp.regatta.Models;

/// <summary>
/// Represents an event in the regatta.
/// </summary>
public class EventModel
{
    /// <summary>
    /// Gets or sets the number of lanes.
    /// </summary>
    public int? Lanes { get; set; }

    /// <summary>
    /// Gets or sets the event identifier.
    /// </summary>
    [JsonPropertyName("ID")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or sets the event end date.
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Gets or sets the event start date.
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Gets or sets the event title.
    /// </summary>
    public string? Title { get; set; }
}
