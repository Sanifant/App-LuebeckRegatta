namespace de.openelp.regatta.Models;

/// <summary>
/// Represents boat data for race endpoints.
/// </summary>
public class BoatDto
{
    /// <summary>
    /// Gets or sets the boat number.
    /// </summary>
    public int? Number { get; set; }

    /// <summary>
    /// Gets or sets the team name.
    /// </summary>
    public string? Team { get; set; }

    /// <summary>
    /// Gets or sets the short team name.
    /// </summary>
    public string? TeamShort { get; set; }

    /// <summary>
    /// Gets or sets athletes as provided by the API.
    /// </summary>
    public string? Athletes { get; set; }

    /// <summary>
    /// Gets or sets the cox.
    /// </summary>
    public string? Cox { get; set; }
}
