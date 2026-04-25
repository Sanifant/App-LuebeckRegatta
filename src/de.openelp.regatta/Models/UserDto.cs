using System.Collections.Generic;

namespace de.openelp.regatta.Models;

/// <summary>
/// Represents an authenticated user.
/// </summary>
public class UserDto
{
    /// <summary>
    /// Gets or sets the first name.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Gets or sets the email address.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets assigned roles.
    /// </summary>
    public List<string>? Roles { get; set; }

    /// <summary>
    /// Gets or sets events assigned to the user.
    /// </summary>
    public List<EventModel>? Events { get; set; }
}
