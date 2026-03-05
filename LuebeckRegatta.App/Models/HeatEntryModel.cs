namespace LuebeckRegatta.App.Models;

/// <summary>
/// Represents a heat entry for a team/athlete in a race
/// </summary>
public class HeatEntryModel
{
    public int? Id { get; set; }
    public int? Lane { get; set; }
    public string? ShortTeamName { get; set; }
    public string? TeamName { get; set; }
    public List<string> Athletes { get; set; } = new();
    public string? Status { get; set; }
    public DateTime? EndDate { get; set; }

    public string AthletesDisplay => string.Join(", ", Athletes);
}
