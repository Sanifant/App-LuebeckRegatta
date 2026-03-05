namespace LuebeckRegatta.App.Models;

/// <summary>
/// Represents a referee in the regatta system
/// </summary>
public class RefereeModel
{
    public int? Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public string FullName => $"{FirstName} {LastName}".Trim();
}
