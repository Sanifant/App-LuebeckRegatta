namespace de.openelp.regatta.Models;

/// <summary>
/// Represents login request data.
/// </summary>
public class LoginRequestDto
{
    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    public string? Password { get; set; }
}
