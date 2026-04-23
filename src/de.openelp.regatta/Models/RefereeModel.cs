using System.Linq;
using System.Text.Json.Serialization;

namespace de.openelp.regatta.Models;

/// <summary>
/// Represents a referee in the regatta system
/// </summary>
public class RefereeModel
{
    /// <summary>
    /// Gets or sets the referee ID
    /// </summary>
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or sets the referee's first name
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Gets or sets the referee's last name
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Gets the formatted display name shown in selectors
    /// </summary>
    public string DisplayName => string.Join(" ", new[] { FirstName, LastName }.Where(s => !string.IsNullOrWhiteSpace(s)));
}
