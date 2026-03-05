namespace LuebeckRegatta.App.Models;

/// <summary>
/// Represents a race heat with all its details and entries
/// </summary>
public class RaceHeatModel
{
    public DateTime? PlannedStartDate { get; set; }
    public string? RaceNumber { get; set; }
    public int? HeatID { get; set; }
    public DateTime? ActualStartDate { get; set; }
    public int? Id { get; set; }
    public string? Title { get; set; }
    public string? Comment { get; set; }
    public string? Status { get; set; }
    public List<HeatEntryModel>? Entries { get; set; }
}
