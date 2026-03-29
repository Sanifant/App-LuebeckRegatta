using System;
using System.Collections.Generic;

namespace de.openelp.regatta.Models;

/// <summary>
/// Represents a team entry in a race heat
/// </summary>
public class HeatEntryModel
{
    /// <summary>
    /// Gets or sets the entry ID
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Gets or sets the lane number
    /// </summary>
    public int? Lane { get; set; }

    /// <summary>
    /// Gets or sets the short team name
    /// </summary>
    public string? ShortTeamName { get; set; }

    /// <summary>
    /// Gets or sets the full team name
    /// </summary>
    public string? TeamName { get; set; }

    /// <summary>
    /// Gets or sets the list of athletes
    /// </summary>
    public List<string> Athletes { get; set; } = new();

    /// <summary>
    /// Gets or sets the entry status
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Gets or sets the end date and time
    /// </summary>
    public DateTime? EndDate { get; set; }
}
