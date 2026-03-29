namespace de.openelp.regatta.Models;

/// <summary>
/// Represents a referee in the regatta system
/// </summary>
public class RefereeModel
{
    /// <summary>
    /// Gets or sets the referee ID
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Gets or sets the referee's first name
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Gets or sets the referee's last name
    /// </summary>
    public string? LastName { get; set; }
}
