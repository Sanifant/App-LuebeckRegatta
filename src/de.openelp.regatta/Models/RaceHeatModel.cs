using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace de.openelp.regatta.Models;

/// <summary>
/// Represents a race heat in the regatta
/// </summary>
public class RaceHeatModel
{
    /// <summary>
    /// Gets or sets the planned start date and time
    /// </summary>
    public DateTime? PlannedStartDate { get; set; }

    /// <summary>
    /// Gets or sets the race number
    /// </summary>
    public string? RaceNumber { get; set; }

    /// <summary>
    /// Gets or sets the heat ID
    /// </summary>
    public int? HeatID { get; set; }

    /// <summary>
    /// Gets or sets the actual start date and time
    /// </summary>
    public DateTime? ActualStartDate { get; set; }

    /// <summary>
    /// Gets or sets the race ID
    /// </summary>
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or sets the race title
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the race comment
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// Gets or sets the race status
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Gets or sets the referee ID
    /// </summary>
    public int? RefereeId { get; set; }

    /// <summary>
    /// Gets or sets the finish referee ID
    /// </summary>
    public int? FinishRefereeId { get; set; }

    /// <summary>
    /// Gets or sets the list of heat entries
    /// </summary>
    public List<HeatEntryModel>? Entries { get; set; }
}
